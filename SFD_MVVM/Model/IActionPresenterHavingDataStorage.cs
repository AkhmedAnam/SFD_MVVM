using System.Collections.Generic;

namespace SFD_MVVM.Model
{
    interface IActionPresenterHavingDataStorage<TStorage>
    {
        List<TStorage> Data { get; set; }
    }
}
