using System;
using System.Collections.Generic;

namespace Nysa.Logics
{

    public record StateValue<S, V>(S State, V Value)
        where S : IEquatable<S>
        where V : IEquatable<V>
    {
    }

}