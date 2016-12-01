using System;

namespace SFD_MVVM.Model
{
    /// <summary>
    ///     Представитель действия, реализующий этот интерфейс,
    ///     имеет конечное состояние и финальное действие, которое
    ///     должно быть выполнено перед удалением этого представителя
    /// </summary>
    interface IActionPresenterHavingFinalState
    {
        void DoFinalAction();

        event EventHandler FinalStateIsReached;
    }
}
