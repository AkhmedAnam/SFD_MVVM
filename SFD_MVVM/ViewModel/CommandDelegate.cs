using System;
using System.Windows.Input;

namespace SFD_MVVM.ViewModel
{
    public class CommandDelegate : ICommand
    {
        public CommandDelegate(Action<object> act)
        {
            ExecuteDelegate = act;
        }

        public CommandDelegate(Action<object> act, Func<object, bool> predicate)
            : this(act)
        {
            CanExecuteDelegate = predicate;
        }

        public Action<object> ExecuteDelegate { get; set; }
        public Func<object, bool> CanExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parametr)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate(parametr);
        }

        public bool CanExecute(object parametr)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parametr);

            return true;
        }
    }
}
