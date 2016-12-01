using System.Collections.ObjectModel;

namespace SFD_MVVM.ViewModel
{
    public class CashViewModel
    {
        public CashViewModel()
        {
            this.Data = new ObservableCollection<DataPresenter>();
        }

        public ObservableCollection<DataPresenter> Data { get; set; }


    }
}
