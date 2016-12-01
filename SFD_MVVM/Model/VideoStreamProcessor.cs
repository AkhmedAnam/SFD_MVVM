using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Media;
using FlyCapture2Managed;
using FlyCapture2Managed.Gui;
using SFD_MVVM.ViewModel;

namespace SFD_MVVM.Model
{
    /// <summary>
    /// Центральный класс модели.
    /// 
    /// Функции:
    /// 1) Стримить видео;
    /// 2) Связь с представлениями-моделями (ViewModels). А точнее обновление их свойств, к которым привязанны (с помощью расширения разметки Binding)
    ///    почти все контролы представления (View. Т.е. главного окна View.xaml);
    /// 3) Выполнение всех преобразований над изображениями. Для того существует безопаснаый в отношении потоков словарь m_actions.
    ///    При каждой итерации стрима видео (Цикл While в методе StreamVideo()) выполняются все представители действий в этом словаре.
    ///    Выполняются они параллельно.
    /// </summary>
    public class VideoStreamProcessor
    {
        public VideoStreamProcessor(Common_ViewModel commVM, Calculation_ViewModel calcVM, Stroboscope_ViewModel strobeVM, HDRI_ViewModel hdriVM)
        {
            m_commonViewModel = commVM; m_calculationsViewModel = calcVM; m_stroboscopeViewModel = strobeVM;
            m_HDRI_ViewModel = hdriVM;
            m_commonViewModel.Processor = this; m_calculationsViewModel.Processor = this;
            m_stroboscopeViewModel.Processor = this; m_HDRI_ViewModel.Processor = this;
            m_actions = new ConcurrentDictionary<string, ActionPresenter>();
            m_isHereActionChangingResultImage = false;
            m_isContinue = false;
            m_threadsCoordinator = new AutoResetEvent(false);
            m_rawImg = new ManagedImage();
            m_cameraCtrlDialog = new CameraControlDialog();
        }

        public void DeleteCurrentCamera()
        {
            StopStreaming();
            DeleteCamera();
        }

        public void ShowCameraSettingWindow()
        {
            if (m_cameraCtrlDialog.IsVisible())
                m_cameraCtrlDialog.Hide();
            else
                m_cameraCtrlDialog.Show();
        }

        public bool IsStreamingNow { get { return m_isContinue; } }

        public void StartStreaming()
        {
            if (m_camera.IsConnected())
            {
                m_isContinue = true;
                m_videoStream = new Thread(StreamVideo);
                m_videoStream.Name = "Video stream";
                try
                {
                    m_camera.StartCapture();
                }
                catch (FC2Exception)
                {
                    m_camera.StopCapture();
                    m_camera.StartCapture();
                }
                m_threadsCoordinator = new AutoResetEvent(false);
                m_videoStream.Start();
            }
        }

        public void StopStreaming()
        {
            if (m_isContinue)
            {
                m_isContinue = false;
                m_threadsCoordinator.WaitOne();

                if (m_camera != null)
                    m_camera.StopCapture();
            }
        }

        public void OnLamp()
        {
            StrobeControl sc = new StrobeControl();

            m_camera.SetGPIOPinDirection(2, 1);
            sc.source = 2;
            sc.onOff = true;
            m_camera.SetStrobe(sc);
            sc.source = 2;
            sc.onOff = false;
            m_camera.SetStrobe(sc);

        }

        public void OffLamp()
        {
            StrobeControl sc = new StrobeControl();

            m_camera.SetGPIOPinDirection(2, 0);
            sc.source = 2;
            sc.onOff = false;
            m_camera.SetStrobe(sc);
        }

        public void Strobe()
        {
            StrobeControl sc = new StrobeControl();
            sc.source = 2;
            sc.onOff = true;

            m_camera.SetGPIOPinDirection(2, 0);

            m_camera.WriteRegister(0x110C, 0x80000200);
            m_camera.WriteRegister(0x1138, 0x80007FFF);

            m_camera.SetStrobe(sc);
        }


        private void DeleteCamera()
        {
            if (m_camera != null)
            {
                if (m_camera.IsConnected())
                    m_camera.Disconnect();

                m_camera = null;

                m_commonViewModel.IsCameraChosen = false;
                m_commonViewModel.ResultImageSource = null;
                m_commonViewModel.ShutterValue = 0;
                m_commonViewModel.CameraShutterRangeBegin = 0;
                m_commonViewModel.CameraShutterRangeEnd = 0;
            }
        }

        //public SynchronizedCamera SynchCamera
        //{
        //    get { return m_synchCamera; }
        //}

        public ImageSource LastResultImage
        {
            get { return m_lastResultImage; }

            set
            {
                m_lastResultImage = value;
            }
        }

        public void SetAutoManualModeForSettings(bool isAuto)
        {
            CameraProperty shutterCP = m_camera.GetProperty(PropertyType.Shutter), gainCP = m_camera.GetProperty(PropertyType.Gain),
                           frameRateCP = m_camera.GetProperty(PropertyType.FrameRate);

            if (!isAuto)
            {
                shutterCP.autoManualMode = false;
                gainCP.autoManualMode = false;
                frameRateCP.autoManualMode = false;
            }
            else
            {
                shutterCP.autoManualMode = true;
                gainCP.autoManualMode = true;
                frameRateCP.autoManualMode = true;
            }

            m_camera.SetProperty(shutterCP);
            m_camera.SetProperty(gainCP);
            m_camera.SetProperty(frameRateCP);
        }

        public void ChooseCamera()
        {
            StopStreaming();
            DeleteCamera();

        beginSelecCamera:
            CameraSelectionDialog selectDialog = new CameraSelectionDialog();
            if (selectDialog.ShowModal() == true)
            {
                ManagedPGRGuid[] guids = selectDialog.GetSelectedCameraGuids();

                if (guids.Length < 1)
                {
                    if (MessageBox.Show("You have not selected a camera. Do you want to restart camera selection diaolog?", "No camera", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        goto beginSelecCamera;
                    else
                        Application.Current.Shutdown();
                }
                m_busManager = new ManagedBusManager();

                var interfaceType = m_busManager.GetInterfaceTypeFromGuid(guids[0]);
                if (interfaceType == InterfaceType.GigE)
                    m_camera = new ManagedGigECamera();
                else
                    m_camera = new ManagedCamera();

                m_camera.Connect(guids[0]);

                EmbeddedImageInfo embeddedInfo = m_camera.GetEmbeddedImageInfo();
                embeddedInfo.timestamp.onOff = true;
                embeddedInfo.exposure.onOff = true;
                embeddedInfo.shutter.onOff = true;
                embeddedInfo.gain.onOff = true;
                m_camera.SetEmbeddedImageInfo(embeddedInfo);

                float shutterMin = m_camera.GetPropertyInfo(PropertyType.Shutter).absMin, shutterMax = m_camera.GetPropertyInfo(PropertyType.Shutter).absMax;
                m_commonViewModel.CameraShutterRangeBegin = shutterMin; m_commonViewModel.CameraShutterRangeEnd = shutterMax;
                FC2Config config = m_camera.GetConfiguration();
                config.grabMode = GrabMode.BufferFrames;
                m_camera.SetConfiguration(config);
                m_cameraCtrlDialog.Connect(m_camera);
                m_commonViewModel.IsCameraChosen = true;
                m_commonViewModel.IsStreamingWasStarted = false;
            }
        }

        public void SetShutter(float value)
        {
            if (m_camera != null)
            {
                CameraProperty shutterCp = m_camera.GetProperty(PropertyType.Shutter);
                shutterCp.absValue = value;
                m_camera.SetProperty(shutterCp);
            }
        }

        public void SaveAllImagesInStroboscopeMode()
        {
            ActionPresenter ap;

            if (m_actions.TryGetValue(PossibleActionPresenters.Stroboscope.ToString(), out ap))
            {
                var sap = ap as StrobeActionPresenter;

                if (sap != null)
                    sap.IsNeedSaveAllImages = true;
            }
        }

        public void SaveSingleImageInStroboscopeMode()
        {
            ActionPresenter ap;
            string fileName = "StrobeImage_" + DateTime.Now.Year.ToString() + "_" +
                    DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                    DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + "_" +
                    DateTime.Now.Second.ToString() + ".jpeg";

            if (m_actions.TryGetValue(PossibleActionPresenters.Stroboscope.ToString(), out ap))
            {
                var sap = ap as StrobeActionPresenter;

                if (sap != null)
                {
                    string directory = (sap.DirectoryToSaveFiles == null || sap.DirectoryToSaveFiles == String.Empty) ? Directory.GetCurrentDirectory() : sap.DirectoryToSaveFiles;
                    fileName = directory + @"\" + fileName;
                    CommonMethods.SaveBitmapSourceToFile(this.LastResultImage as BitmapSource, fileName);
                }
            }
        }

        public void SetNewCursorPositionInSelectionMode(Point position)
        {
            if (m_selectDataPresenter != null)
                m_selectDataPresenter.ActionPresenter.CurrentCursorPosition = position;
        }

        public void SetNewVideoStreamAreaActualWidth(double value)
        {
            if (m_selectDataPresenter != null)
                m_selectDataPresenter.ActionPresenter.VideoStreamAreaActualWidth = value;
        }

        public void SetNewVideoStreamAreaActualHeight(double value)
        {
            if (m_selectDataPresenter != null)
                m_selectDataPresenter.ActionPresenter.VideoStreamAreaActualHeight = value;
        }

        public void MemorizeAsBckgroundDataInSelectionMode()
        {
            m_selectDataPresenter.PrepareBackgroundData();
        }

        public void MemorizeAsCOData(double COconcentration)
        {
            m_selectDataPresenter.EtalonConcentration = COconcentration;
            m_selectDataPresenter.PrepareCOData();
        }

        #region AddindActionToTheDictionary

        public void AddVideoRecordingAction(string directory)
        {
            VideoRecordingActionPresenter ap = new VideoRecordingActionPresenter(PossibleActionPresenters.VideoRecording.ToString(), directory);
            ap.FinalStateIsReached += (sender, e) =>
                {
                    m_commonViewModel.IsVideoRecording = false;
                };

            InitializeActionPresenter(ap);
        }

        public void AddHDRI_Action(int framesInSeriaNumber, int seriesNumber, double shutterRangeBegin, double shutterRangeEnd, string directoryToSave = "current")
        {
            if (directoryToSave == "current")
                directoryToSave = Directory.GetCurrentDirectory();
            directoryToSave += @"\";
            HDRI_ActionPresenter ap = new HDRI_ActionPresenter(PossibleActionPresenters.HDRI.ToString(), framesInSeriaNumber, seriesNumber, shutterRangeBegin, shutterRangeEnd, directoryToSave);
            ap.FinalStateIsReached += (sender, e) =>
                {
                    m_HDRI_ViewModel.IsHDRI_Processing = false;
                };

            ap.ChangeCameraProperty += (sender, e) =>
                {
                    CameraProperty cp = m_camera.GetProperty(e.PropertyType);
                    cp.absValue = e.ValueToSet;
                    m_camera.SetProperty(cp);
                    m_commonViewModel.ShutterValue = e.ValueToSet;
                };

            InitializeActionPresenter(ap);
        }

        public void AddSelectAction(Point cursorPosition, double videoStreamAreaActualHeight, double videoStreamActualWidth, int imagesNumberInNormalization)
        {
            SelectAreaActionPresenter ap = new SelectAreaActionPresenter(PossibleActionPresenters.AreaSelection.ToString(), cursorPosition, videoStreamAreaActualHeight, videoStreamActualWidth);
            InitializeActionPresenter(ap);
            m_selectDataPresenter = new SelectionDataPresenter(ap, imagesNumberInNormalization);
            m_selectDataPresenter.CurrentConcentrationChanged += (sender, e) =>
                {
                    SelectionDataPresenter dp = sender as SelectionDataPresenter;

                    if (dp != null) m_calculationsViewModel.CurrentCancentrationValue = Math.Round(dp.CurrentConcentration, 3);
                };
        }

        public void AddStrobeAction(string directoryForFiles)
        {
            StrobeActionPresenter ap = new StrobeActionPresenter(PossibleActionPresenters.Stroboscope.ToString(), directoryForFiles);
            InitializeActionPresenter(ap);
        }

        #endregion

        #region DeletingActionsFromTheDictionary

        public void DeleteSelectAction()
        {
            if (m_selectDataPresenter != null)
            {
                var ap = m_selectDataPresenter.ActionPresenter;

                if (ap != null)
                    ap.IsNeedImage = false;

                m_selectDataPresenter = null;

            }
        }

        public void DeleteVidoeRecordingAction()
        {
            ActionPresenter ap;
            if (m_actions.TryGetValue(PossibleActionPresenters.VideoRecording.ToString(), out ap))
            {
                var videoRecordingAP = ap as VideoRecordingActionPresenter;

                if (videoRecordingAP != null)
                    videoRecordingAP.IsVideoRecording = false;
            }
        }

        public void DeleteStroboscopeAction()
        {
            ActionPresenter ap;

            if (m_actions.TryGetValue(PossibleActionPresenters.Stroboscope.ToString(), out ap))
            {
                ap.IsNeedImage = false;
            }
        }

        #endregion


        private void InitializeActionPresenter(ActionPresenter ap)
        {
            var apChangingImg = ap as IActionPresenterChangingResultImage;

            if (apChangingImg != null)
            {
                if (!m_isHereActionChangingResultImage)
                {
                    m_isHereActionChangingResultImage = true;

                    apChangingImg.NewResultImageIsReady += (sender, e) =>
                    {
                        var apChanginImage = sender as IActionPresenterChangingResultImage;
                        m_commonViewModel.ResultImageSource = apChanginImage.NewResultImage;
                        ImageSource img = apChanginImage.NewResultImage.Clone();
                        img.Freeze();
                        this.LastResultImage = img;
                    };
                }
                else
                    return;
            }

            var apNeedingProperty = ap as IActionPresenterNeedingCameraProperty<float>;

            if (apNeedingProperty != null)
            {
                apNeedingProperty.SetNeededCameraProperty += (sender, e) =>
                    {
                        var sendr = sender as IActionPresenterNeedingCameraProperty<float>;
                        sendr.CurrentNeededPropertyValue = m_camera.GetProperty(sendr.NeededPropertyType).absValue;
                    };
            }

            ap.IsNeedImageChanged += (sender, e) =>
            {
                var apChangingImage = sender as IActionPresenterChangingResultImage;

                if (apChangingImage != null)
                    m_isHereActionChangingResultImage = false;

                var AP = sender as ActionPresenter;

                m_actions.TryRemove(AP.ID, out AP);
            };

            m_actions.AddOrUpdate(ap.ID, ap, (str, AP) => AP.Clone() as ActionPresenter);
        }


        private void StreamVideo()
        {
            while (m_isContinue)
            {
                try
                {
                    m_camera.RetrieveBuffer(m_rawImg);
                }
                catch (FC2Exception ex)
                {
                    StopStreaming();
                    DeleteCamera();
                    m_commonViewModel.IsCameraChosen = false;
                    MessageBox.Show(String.Format("An error has been occurred while getting an image from the camera. Try to initialize a new camera.\n\nAdditional info: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    StopStreaming();
                    DeleteCamera();
                    m_commonViewModel.IsCameraChosen = false;
                    MessageBox.Show(String.Format("An error has been occurred during getting an image from the camera. Try to initialize a new camera.\n\nAdditional info: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (m_actions.Count != 0)
                    ExecuteAllActions(m_rawImg);

                if (!m_isHereActionChangingResultImage)
                {
                    ManagedImage copy = new ManagedImage();
                    m_rawImg.ConvertToBitmapSource(copy);

                    ImageSource img = copy.bitmapsource.Clone();
                    copy.bitmapsource.Freeze();
                    m_commonViewModel.ResultImageSource = copy.bitmapsource;
                    img.Freeze();
                    this.LastResultImage = img;
                }
            }
            m_threadsCoordinator.Set();
        }

        private void ExecuteAllActions(ManagedImage img)
        {
            Parallel.ForEach<ActionPresenter>(m_actions.Values, (ap) => ap.ExecuteAction(img));
        }

        private Thread m_videoStream;
        private SelectionDataPresenter m_selectDataPresenter;
        private ManagedImage m_rawImg;
        private ConcurrentDictionary<string, ActionPresenter> m_actions;
        private ManagedCameraBase m_camera;
        private ManagedBusManager m_busManager;
        //private readonly SynchronizedCamera m_synchCamera;
        private volatile bool m_isHereActionChangingResultImage, m_isContinue;
        private AutoResetEvent m_threadsCoordinator;
        private CameraControlDialog m_cameraCtrlDialog;
        private ImageSource m_lastResultImage;

        #region ViewModelsReferences

        private readonly Common_ViewModel m_commonViewModel;
        private readonly Calculation_ViewModel m_calculationsViewModel;
        private readonly Stroboscope_ViewModel m_stroboscopeViewModel;
        private readonly HDRI_ViewModel m_HDRI_ViewModel;

        #endregion
    }

    public enum PossibleActionPresenters
    {
        HDRI,
        VideoRecording,
        Stroboscope,
        AreaSelection
    }
}
