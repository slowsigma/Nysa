using System;
using System.Windows.Input;

namespace Nysa.Windows.Input
{

    public class SimpleCommand<T> : ICommand
    {
        public Predicate<T>     CanExecuteDelegate  { get; set; }
        public Action<T>        ExecuteDelegate     { get; set; }

        public Boolean CanExecute(T parameter)
        {
            if (this.CanExecuteDelegate != null) return this.CanExecuteDelegate(parameter);

            return true;
        }

        public Boolean CanExecute(Object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(T parameter)
        {
            if (this.ExecuteDelegate != null) this.ExecuteDelegate(parameter);
        }

        public void Execute(Object parameter)
        {
            this.Execute((T)parameter);
        }
    }

}
