using System.Windows.Input;


namespace SFD_MVVM.ViewModel
{
    public class Common_ViewModel : ViewModelBase
    {
        public Common_ViewModel()
        {
            this.StartStreaming = new CommandDelegate(StartStreamingWork, (o) => this.IsStreamingWasStarted == false);
            this.StopStreaming = new CommandDelegate(StopStreamingWork, (o) => this.IsStreamingWasStarted == true);
            this.OpenCameraSettingsWindow = new CommandDelegate(ShowCameraSettingWindow);
            this.SetAutoManualMode = new CommandDelegate(SetAutoManualCameraPropertiesControlling);
            this.ChooseCameraCommand = new CommandDelegate(ChooseCamera);
            this.StartRecordingVideoCommand = new CommandDelegate(StartRecordingVideo, IsStartRecordingVideoCommandCanExecute);
            this.StopRecordingVideoCommand = new CommandDelegate(StopRecordingVideo, IsStopRecordingVideoCommandCanExecute);
            this.RemoveCurrentCamera = new CommandDelegate(RemoveCurrentCameraWork, CanExecuteRemoveCurrentCamera);
        }

        #region Properties

        public System.Windows.Media.ImageSource ResultImageSource
        {
            get { return m_resultImageSource; }

            set
            {
                m_resultImageSource = value;
                OnPropertyChanged(this, "ResultImageSource");
            }
        }

        public bool IsCameraChosen
        {
            get { return m_isCameraChosen; }

            set
            {
                m_isCameraChosen = value;
                OnPropertyChanged(this, "IsCameraChosen");
            }
        }

        public bool? IsStreamingWasStarted { get; set; }

        public bool IsVideoRecording
        {
            get
            {
                return m_IsVideoRecordingStarted;
            }

            set
            {
                m_IsVideoRecordingStarted = value;
                OnPropertyChanged(this, "IsVideoRecording");
            }
        }

        public string VideoSavePath
        {
            get
            {
                return m_VideoSavePath;
            }

            set
            {
                m_VideoSavePath = value;
                OnPropertyChanged(this, "VideoSavePath");
            }
        }

        public double ShutterValue
        {
            get { return m_shutterValue; }

            set
            {
                m_shutterValue = value;
                if(m_processor != null)
                    m_processor.SetShutter((float)m_shutterValue);
                OnPropertyChanged(this, "ShutterValue");
            }
        }

        public double CameraShutterRangeBegin
        {
            get { return m_cameraShutterRangeBegin; }

            set
            {
                m_cameraShutterRangeBegin = value;
                OnPropertyChanged(this, "CameraShutterRangeBegin");
            }
        }

        public double CameraShutterRangeEnd
        {
            get { return m_cameraShutterRangeEnd; }

            set
            {
                m_cameraShutterRangeEnd = value;
                OnPropertyChanged(this, "CameraShutterRangeEnd");
            }
        }

        #endregion Properties

        #region Commands

        public ICommand OpenCameraSettingsWindow { get; set; }

        public ICommand ChooseCameraCommand { get; set; }

        public ICommand StartStreaming { get; set; }

        public ICommand StopStreaming { get; set; }

        //internal ICommand ChoosePathToSaveHDRImage { get; set; }

        //internal ICommand ChoosePathToSaveStroboscopeImage { get; set; }

        //internal ICommand ChoosePathToSaveVideo { get; set; }

        public ICommand SetAutoManualMode { get; set; }

        public ICommand StartRecordingVideoCommand { get; set; }

        public ICommand StopRecordingVideoCommand { get; set; }

        public ICommand RemoveCurrentCamera { get; set; }

        #endregion Commands

        private void StartStreamingWork(object param)
        {
            if (m_processor != null)
            {
                m_processor.StartStreaming();
                this.IsStreamingWasStarted = true;
            }
        }
        private void StopStreamingWork(object param)
        {
            if (m_processor != null)
            {
                m_processor.StopStreaming();
                this.IsStreamingWasStarted = false;
            }
        }
        private void StartRecordingVideo(object param)
        {
            this.IsVideoRecording = true;
            m_processor.AddVideoRecordingAction(this.VideoSavePath);
        }
        private bool IsStartRecordingVideoCommandCanExecute(object o)
        {
            return !this.IsVideoRecording;
        }
        private bool IsStopRecordingVideoCommandCanExecute(object o)
        {
            return this.IsVideoRecording;
        }
        private void StopRecordingVideo(object param)
        {
            m_processor.DeleteVidoeRecordingAction();
        }
        private void ShowCameraSettingWindow(object param)
        {
            m_processor.ShowCameraSettingWindow();
        }
        private void SetAutoManualCameraPropertiesControlling(object param)
        {
            bool isAuto = (bool)param;

            m_processor.SetAutoManualModeForSettings(isAuto);
        }
        private void ChooseCamera(object param)
        {
            m_processor.ChooseCamera();
        }
        private void RemoveCurrentCameraWork(object o)
        {
            m_processor.DeleteCurrentCamera();
        }
        private bool CanExecuteRemoveCurrentCamera(object o)
        {
            return this.IsCameraChosen;
        }


        private System.Windows.Media.ImageSource m_resultImageSource;
        private bool m_IsVideoRecordingStarted, m_isCameraChosen;
        private string m_VideoSavePath;
        private double m_shutterValue, m_cameraShutterRangeBegin, m_cameraShutterRangeEnd;

        

    }
}
