using System;
using System.Windows.Input;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class Command : ICommand
{
    private Action<Object?>         _Execute;
    private Func<Object?, Boolean>? _CanExecute;

    public Command(Action execute, Func<Object?, Boolean>? canExecute = null)
    {
        this._Execute    = (o) => execute();
        this._CanExecute = canExecute;
    }

    public Command(Action<Object?> execute, Func<Object?, Boolean>? canExecute = null)
    {
        this._Execute = execute;
        this._CanExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return this._CanExecute != null
               ? this._CanExecute(parameter)
               : true;
    }

    public void Execute(object? parameter)
    {
        this._Execute(parameter);
    }
}