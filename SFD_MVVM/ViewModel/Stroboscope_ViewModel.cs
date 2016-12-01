using System.Windows.Input;
using System.IO;

namespace SFD_MVVM.ViewModel
{
    public class Stroboscope_ViewModel : ViewModelBase
    {
        public Stroboscope_ViewModel()
        {
            this.DirectoryForFiles = Directory.GetCurrentDirectory();
            this.StrobeModeClickCommand = new CommandDelegate(UseStrobe);
            this.StrobeCommand = new CommandDelegate(Strobe, (obj) => this.IsNeedUseStroboscope);
            this.OnLampCommand = new CommandDelegate(OnLamp, (obj) => this.IsNeedUseStroboscope);
            this.OffLampCommand = new CommandDelegate(OffLamp, (obj) => this.IsNeedUseStroboscope);
            this.SaveAllImagesCommand = new CommandDelegate(SaveAllImages, (obj) => this.IsNeedUseStroboscope);
            this.SaveSingleImageCommand = new CommandDelegate(SaveSingleImage, (obj) => this.IsNeedUseStroboscope && this.IsNeedSaveSingleImage);
        }

        public bool IsNeedUseStroboscope
        {
            get { return m_isNeedUseStroboscope; }

            set
            {
                m_isNeedUseStroboscope = value;
                OnPropertyChanged(this, "IsNeedUseStroboscope");
            }
        }

        public bool IsNeedSaveSingleImage
        {
            get { return m_isNeedToSaveSingleImage; }

            set
            {
                m_isNeedToSaveSingleImage = value;
                OnPropertyChanged(this, "IsNeedSaveSingleImage");
            }
        }

        public string DirectoryForFiles { get; set; }

        public ICommand StrobeModeClickCommand { get; set; }

        public ICommand StrobeCommand { get; set; }

        public ICommand OnLampCommand { get; set; }

        public ICommand OffLampCommand { get; set; }

        public ICommand SaveSingleImageCommand { get; set; }

        public ICommand SaveAllImagesCommand { get; set; }

        private void UseStrobe(object o)
        {
            bool t_isChecked = (bool)o;
            this.IsNeedUseStroboscope = t_isChecked;

            if (t_isChecked)
                m_processor.AddStrobeAction(this.DirectoryForFiles);
            else
                m_processor.DeleteStroboscopeAction();
        }
        private void Strobe(object o)
        {
            m_processor.Strobe();
        }
        private void OnLamp(object o)
        {
            m_processor.OnLamp();
        }
        private void OffLamp(object o)
        {
            m_processor.OffLamp();
        }
        private void SaveAllImages(object o)
        {
            bool t_isChecked = (bool)o;

            if (t_isChecked)
                m_processor.SaveAllImagesInStroboscopeMode();
        }
        private void SaveSingleImage(object o)
        {
            m_processor.SaveSingleImageInStroboscopeMode();
        }

        private bool m_isNeedUseStroboscope, m_isNeedToSaveSingleImage;
    }
}
