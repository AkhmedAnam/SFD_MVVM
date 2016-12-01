using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Windows.Input;
using SFD_MVVM.Model;
using SFD_MVVM.View;

namespace SFD_MVVM.ViewModel
{
    public class ExperementalDataFormViewModel : ViewModelBase
    {
        public ExperementalDataFormViewModel()
        {
            this.SaveButtonClick = new CommandDelegate(SaveClick, IsSaveButtonValid);
        }

        #region Properties

        public int MouseNumber
        {
            get { return m_mouseNumber; }

            set
            {
                m_mouseNumber = value;
                OnPropertyChanged(this, "MouseNumber");
            }
        }

        public Organs Organ
        {
            get { return m_organ; }

            set
            {
                m_organ = value;
                OnPropertyChanged(this, "Organ");
            }
        }

        public double Dose
        {
            get { return m_dose; }

            set
            {
                m_dose = value;
                OnPropertyChanged(this, "Dose");
            }
        }

        public double CurrentConcentration
        {
            get
            {
                return m_currentConcentration;
            }

            set
            {
                m_currentConcentration = value;
                OnPropertyChanged(this, "CurrentConcentration");
            }
        }

        public TimeSpan TimeSpanProp
        {
            get { return m_timeSpan; }

            set
            {
                m_timeSpan = value;
                OnPropertyChanged(this, "TimeSpan");
            }
        }

        public SelectionDataPresenter SelectionDataPresenterProp
        {
            set
            {
                m_selectionDataPresenter = value;
            }
        }

        #endregion

        #region Commands

        public ICommand SaveButtonClick { get; set; }

        #endregion

        private void SaveClick(object o)
        {
            var window = o as ExperementalDataFormView;

            string fullFileName = Directory.GetCurrentDirectory() + @"\" + m_directoryToSaveImages + "DataImge_" + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.Month.ToString() + "_"
                + DateTime.Now.Year.ToString() + "("
                + DateTime.Now.Hour.ToString() + "_"
                + DateTime.Now.Minute.ToString() + "_"
                + DateTime.Now.Second.ToString() + ").bmp";

            BitmapSource src = window.Img.Source as BitmapSource;

            CommonMethods.SaveBitmapSourceToFile(src, fullFileName);

            if (File.Exists(m_fileNameToSaveData))
            {
                XDocument xDoc = XDocument.Load(m_fileNameToSaveData);

                xDoc.Root.Add(new XElement("Мышь",
                    new XElement("Номер", this.MouseNumber),
                    new XElement("Орган", this.Organ),
                    new XElement("ВведённаяДоза", this.Dose),
                    new XElement("ТекущаяКонцентрация", this.CurrentConcentration),
                    new XElement("ПрошедшееВремя", this.TimeSpanProp.ToString()),
                    new XElement("ПутьКИзображению", fullFileName)));

                xDoc.Save(m_fileNameToSaveData);
            }
            else
            {
                XDocument xDoc = new XDocument();

                XElement root = new XElement("Мыши",
                new XElement("Мышь",
                    new XElement("Номер", this.MouseNumber),
                    new XElement("Орган", this.Organ),
                    new XElement("ВведённаяДоза", this.Dose),
                    new XElement("ТекущаяКонцентрация", this.CurrentConcentration),
                    new XElement("ПрошедшееВремя", this.TimeSpanProp.ToString()),
                    new XElement("ПутьКИзображению", fullFileName)));

                xDoc.Add(root);

                xDoc.Save(m_fileNameToSaveData);
            }

            window.Close();
        }

        private bool IsSaveButtonValid(object o)
        {
            
            return (this.MouseNumber > 0 && this.Dose != Double.NaN && this.Dose > 0 && this.TimeSpanProp != TimeSpan.Zero);
        }

        private int m_mouseNumber;
        private Organs m_organ;
        public double m_dose, m_currentConcentration;
        private TimeSpan m_timeSpan;
        private SelectionDataPresenter m_selectionDataPresenter;
        private const string m_fileNameToSaveData = @"Calculation\Experimental_Data\XML_Files\DataTree.xml",
             m_directoryToSaveImages = @"Calculation\Experimental_Data\Images\";
    }

    public enum Organs
    {
        Heart,
        Kidneys, //почки
        Liver
    }
}
