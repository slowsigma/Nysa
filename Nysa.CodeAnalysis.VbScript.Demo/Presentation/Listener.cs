using System;
using System.Threading;

namespace Nysa.CodeAnalysis.VbScript.Demo;

// The type parameter represents the type of value who's change triggers the Listener
public abstract class Listener<T>
{
    private ManualResetEvent _LogicCompleted;
    
    public Listener()
    {
        this._LogicCompleted = new ManualResetEvent(false);
    }

    public void SetIncomplete()
    {
        this._LogicCompleted.Reset();
    }

    public void ValueChanged(T value, Int32 version)
    {
        this.OnValueChanged(value, version);

        this._LogicCompleted.Set();
    }

    // implement this, don't allow exceptions:
    protected abstract void OnValueChanged(T value, Int32 version);

    // before accessing state affected by OnValueChanged, call this:
    public Boolean IsBackgroundComplete()
    {
        this._LogicCompleted.WaitOne();

        return true;
    }
}