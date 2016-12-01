using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SFD_MVVM.View;

namespace SFD_MVVM.ViewModel
{
    public class Calculation_ViewModel : ViewModelBase
    {
        public Calculation_ViewModel()
        {
            this.BackgroundData = new List<DataPresenter>();
            this.ControlSamplesData = new List<DataPresenter>();
            m_initialSelectingAreaLength = 10;
            this.SelectingAreaLength = m_initialSelectingAreaLength;
            this.NumberOfImagesInNormalization = 3;
            m_cursorPosition = new Point(0, 0);
            this.SelectionModeClickCommand = new CommandDelegate(ActivateSelectionMode);
            this.MemorizeAsBackgroundDataCommand = new CommandDelegate(MemorizeAsBckrndData);
            this.MemorizeAsControlSampleDataCommand = new CommandDelegate(MemorizeAsCOData);
            this.SaveAsBackgroundDataCommand = new CommandDelegate(SaveAsBackgroundData);
            this.SaveAsControlSampleDataCommand = new CommandDelegate(SaveAsControlSampleData);
            this.SaveAsDataCommand = new CommandDelegate(SaveAsData);
            this.SeeBackgroundDataCommand = new CommandDelegate(SeeBackgroundData);
            this.SeeControlSampleDataCommand = new CommandDelegate(SeeControlSampleData);
            this.SeeExperimantalDataCommand = new CommandDelegate(SeeExperimantalData);
            m_dataFormWindow = new ExperementalDataFormView();
        }

        #region Properties

        public int SelectingAreaLength
        {
            get { return m_selectingAreaLength; }

            set
            {
                m_selectingAreaLength = value;
                OnPropertyChanged(this, "SelectingAreaLength");
            }
        }

        public int NumberOfImagesInNormalization
        {
            get { return m_numberOfImagesInNormalization; }

            set
            {
                m_numberOfImagesInNormalization = value;
                OnPropertyChanged(this, "NumberOfImagesInNormalization");
            }
        }

        public double ControlSampleCancentrationValue
        {
            get { return m_controlSampleConcentrationValue; }

            set
            {
                m_controlSampleConcentrationValue = value;
                OnPropertyChanged(this, "ControlSampleCancentrationValue");
            }
        }

        public double CurrentCancentrationValue
        {
            get { return m_currentConcentrationValue; }

            set
            {
                m_currentConcentrationValue = value;
                OnPropertyChanged(this, "CurrentCancentrationValue");
            }
        }

        public double VideoStreamAreaActualHeight
        {
            get { return m_videoStreamAreaActualHeight; }

            set
            {
                m_videoStreamAreaActualHeight = value;

                if (m_processor != null)
                    m_processor.SetNewVideoStreamAreaActualHeight(value);
            }
        }

        public double VideoStreamAreaActualWidth
        {
            get { return m_videoStreamAreaActualWidth; }

            set
            {
                m_videoStreamAreaActualWidth = value;

                if (m_processor != null)
                    m_processor.SetNewVideoStreamAreaActualWidth(value);
            }
        }

        public Point CursorPosition
        {
            get { return m_cursorPosition; }

            set
            {
                m_cursorPosition = value;
                m_processor.SetNewCursorPositionInSelectionMode(m_cursorPosition);
            }
        }

        //public bool? SelectionModeOn
        //{
        //    get { return m_selectionModeOn; }

        //    set
        //    {
        //        m_selectionModeOn = value;
        //        OnPropertyChanged(this, "SelectionModeOn");
        //    }
        //}

        public List<DataPresenter> ControlSamplesData { get; set; }

        public List<DataPresenter> BackgroundData { get; set; }

        #endregion Properties

        #region Commands

        public ICommand SelectionModeClickCommand { get; set; }

        public ICommand MemorizeAsControlSampleDataCommand { get; set; }

        public ICommand MemorizeAsBackgroundDataCommand { get; set; }

        public ICommand SaveAsBackgroundDataCommand { get; set; }

        public ICommand SaveAsControlSampleDataCommand { get; set; }

        public ICommand SaveAsDataCommand { get; set; }

        public ICommand SeeExperimantalDataCommand { get; set; }

        public ICommand SeeControlSampleDataCommand { get; set; }

        public ICommand SeeBackgroundDataCommand { get; set; }

        #endregion Commands

        

        private void ActivateSelectionMode(object obj)
        {
            bool isChecked = (bool)obj;

            if (isChecked)//if 'use selection mode' check box is checked
            {
                m_processor.AddSelectAction(m_cursorPosition, this.VideoStreamAreaActualHeight, this.VideoStreamAreaActualWidth, m_numberOfImagesInNormalization);
            }
            else//if 'use selection mode' check box is NOT checked
            {
                m_processor.DeleteSelectAction();
            }
        }

        private void MemorizeAsBckrndData(object obj)
        {
            m_processor.MemorizeAsBckgroundDataInSelectionMode();
        }

        private void MemorizeAsCOData(object obj)
        {
            m_processor.MemorizeAsCOData(this.ControlSampleCancentrationValue);
        }

        private void SaveAsBackgroundData(object parametr)
        {
            BitmapSource imgSrc = m_processor.LastResultImage as BitmapSource;
            if (!imgSrc.IsFrozen)
                imgSrc.Freeze();
            DataPresenter dp = new DataPresenter();

            string fullFileName = Directory.GetCurrentDirectory() + @"\" + m_backgroundImagesDirectory + "BackgroundImage" + "_" + DateTime.Now.Day.ToString()
                + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_(" + DateTime.Now.Hour.ToString()
                + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ").bmp";

            dp.ImagePath = fullFileName;
            dp.Description = String.Empty;

            CommonMethods.SaveBitmapSourceToFile(imgSrc, fullFileName);

            this.BackgroundData.Add(dp);
        }

        private void SaveAsControlSampleData(object parametr)
        {
            BitmapSource imgSrc = m_processor.LastResultImage as BitmapSource;
            
            DataPresenter dataPresenter = new DataPresenter();

            string fullFileName = Directory.GetCurrentDirectory() + @"\" + m_COImagesDirectory + "CheckObject" + "_" + DateTime.Now.Day.ToString()
                + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_( " + DateTime.Now.Hour.ToString()
                + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ").bmp";

            dataPresenter.ImagePath = fullFileName;

            CommonMethods.SaveBitmapSourceToFile(imgSrc, fullFileName);

            dataPresenter.Description = String.Format("Concentration: {0}", this.CurrentCancentrationValue.ToString());

            this.ControlSamplesData.Add(dataPresenter);
        }

        private void SaveAsData(object parametr)
        {
            ExperementalDataFormView formToBeFillIn = new ExperementalDataFormView();
            BitmapSource imgSrc = m_processor.LastResultImage as BitmapSource;
            formToBeFillIn.Image = imgSrc;
            ExperementalDataFormViewModel vm = formToBeFillIn.Resources["dataVM"] as ExperementalDataFormViewModel;
            vm.CurrentConcentration = this.CurrentCancentrationValue;
            formToBeFillIn.Show();
        }

        private void SeeBackgroundData(object parametr)
        {
            CashWindow cw = new CashWindow();
            CashViewModel cVM = cw.Resources["VM"] as CashViewModel;
            
            if (cVM != null)
            {
                foreach (var item in this.BackgroundData)
                    cVM.Data.Add(item);
            }

            cw.Show();
        }

        private void SeeControlSampleData(object parametr)
        {
            CashWindow cw = new CashWindow();
            CashViewModel cVM = cw.Resources["VM"] as CashViewModel;

            if (cVM != null)
            {
                foreach (var item in this.ControlSamplesData)
                    cVM.Data.Add(item);
            }

            cw.Show();
        }

        private void SeeExperimantalData(object parametr)
        {
            CashWindow cw = new CashWindow();
            CashViewModel cVM = cw.Resources["VM"] as CashViewModel;

            if (cVM != null)
            {
                if (File.Exists(m_fileNameToGetData))
                {
                    XDocument xDoc = XDocument.Load(m_fileNameToGetData);

                    IEnumerable<DataPresenter> data = from i in xDoc.Element("Мыши").Elements("Мышь")
                                                      select new DataPresenter
                                                      {
                                                          ImagePath = i.Element("ПутьКИзображению").Value,
                                                          Description = String.Format("Mouse number: {0}\nOrgan: {1}\nInjected dose (mg/kg): {2}\nCurrent concentration (mg/kg): {3}\nTime interval (hh:mm:ss): {4}",
                                                          i.Element("Номер").Value, i.Element("Орган").Value, i.Element("ВведённаяДоза").Value, i.Element("ТекущаяКонцентрация").Value, i.Element("ПрошедшееВремя").Value)
                                                      };

                    foreach (var item in data)
                        cVM.Data.Add(item);

                }
            }

            cw.Show();
        }

        private string TranslateOrganToRussian(string organInEnglish)
        {
            switch (organInEnglish)
            {
                case "Heart":
                    return "Сердце";
                case "Kidneys":
                    return "Почки";
                case "Liver":
                    return "Печень";
                default:
                    return "Неизвестный орган";
            }
        }

        private int m_selectingAreaLength, m_numberOfImagesInNormalization, m_initialSelectingAreaLength;
        private double m_controlSampleConcentrationValue, m_currentConcentrationValue,
                      m_videoStreamAreaActualHeight, m_videoStreamAreaActualWidth;
        //private bool? m_selectionModeOn;
        private Point m_cursorPosition;
        private ExperementalDataFormView m_dataFormWindow;
        private const string m_backgroundImagesDirectory = @"Calculation\Background\Images\",
            m_COImagesDirectory = @"Calculation\CO_Data\Images\",
            m_fileNameToGetData = @"Calculation\Experimental_Data\XML_Files\DataTree.xml";
    }

    public struct DataPresenter
    {
        public string ImagePath { get; set; }

        public string Description { get; set; }
    }

}
