using System;
using System.Windows.Input;

namespace Nysa.Windows.Input
{

    public class SimpleCommand : ICommand
    {
        public static SimpleCommand Create<T>(Action<Object> action)
            => new SimpleCommand() { CanExecuteDelegate = null, ExecuteDelegate = action };

        public static SimpleCommand Create<T>(Predicate<Object> predicate, Action<Object> action)
            => new SimpleCommand() { CanExecuteDelegate = predicate, ExecuteDelegate = action };

        public static SimpleCommand<T> Create<T>(Action<T> action)
            => new SimpleCommand<T>() { CanExecuteDelegate = null, ExecuteDelegate = action };

        public static SimpleCommand<T> Create<T>(Predicate<T> predicate, Action<T> action)
            => new SimpleCommand<T>() { CanExecuteDelegate = predicate, ExecuteDelegate = action };

        
        // instance members
        public Predicate<Object>    CanExecuteDelegate  { get; set; }
        public Action<Object>       ExecuteDelegate     { get; set; }

        public Boolean CanExecute(Object parameter)
        {
            if (this.CanExecuteDelegate != null) return this.CanExecuteDelegate(parameter);

            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(Object parameter)
        {
            if (this.ExecuteDelegate != null) this.ExecuteDelegate(parameter);
        }
    }

}
