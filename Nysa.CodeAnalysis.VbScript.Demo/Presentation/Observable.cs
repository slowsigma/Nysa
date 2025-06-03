using System;
using System.Threading;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class Observable<T>
    where T : IEquatable<T>
{
    private Boolean _Disposed;
    private T _Value;
    private IReadOnlyList<Listener<T>> _Listeners;
    private Thread _Background;

    private Int32 _Version;

    public Int32 Version => this._Version;

    public Observable(T value, params Listener<T>[] listeners)
    {
        this._Disposed = false;
        this._Value = value;
        this._Listeners = listeners;
        this._Background = new Thread(this.Run);
        this._Background.IsBackground = true;
        this._Background.Start();
        this._Version = 0;
    }

    // do not set this too often, if _HasChanged is still true, it will block until background logic completes
    public T Value
    {
        get { return this._Value; }
        set
        {
            if (!this._Value.Equals(value))
            {
                this._Background.Join(); // wait on background thread

                foreach (var listener in this._Listeners)
                    listener.SetIncomplete();

                // set this state
                this._Value = value;
                Interlocked.Increment(ref this._Version);
                // run background
                this._Background = new Thread(this.Run);
                this._Background.IsBackground = true;
                this._Background.Start();
            }
        }
    }

    private void Run()
    {
        foreach (var listener in this._Listeners)
            listener.ValueChanged(this._Value, this._Version);
    }

}