using System;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public static class ObservableFunctions
{

    public static Observable<T> ToObservable<T>(this T value, params Listener<T>[] listeners)
        where T : IEquatable<T>
    {
        return new Observable<T>(value, listeners);
    }

}