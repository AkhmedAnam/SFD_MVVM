using System;
using System.ComponentModel;
using SFD_MVVM.Model;


namespace SFD_MVVM.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
        }

        public VideoStreamProcessor Processor
        {
            get { return m_processor; }
            set { m_processor = value; }
        }

        protected VideoStreamProcessor m_processor;
    }
}
