using System;
using System.Collections.Generic;

namespace Nysa.Collections.Generic;

public class BidirectionalIndex<TOne, TTwo>
    where TOne : IEquatable<TOne>
    where TTwo : IEquatable<TTwo>
{
    private Dictionary<TOne, TTwo> _Forward;
    private Dictionary<TTwo, TOne> _Backward;

    public BidirectionalIndex(IEqualityComparer<TOne> oneComparer, IEqualityComparer<TTwo> twoComparer)
    {
        this._Forward  = new Dictionary<TOne, TTwo>(oneComparer);
        this._Backward = new Dictionary<TTwo, TOne>(twoComparer);
    }

    public void Add(TOne one, TTwo two)
    {
        this._Forward.Add(one, two);
        this._Backward.Add(two, one);
    }

    public Int32 Count => this._Forward.Count;

    public Boolean ContainsOne(TOne one) => this._Forward.ContainsKey(one);
    public Boolean ContainsTwo(TTwo two) => this._Backward.ContainsKey(two);

    public TTwo GetTwo(TOne one) => this._Forward[one];
    public TOne GetOne(TTwo two) => this._Backward[two];

    public IEnumerable<TOne> Ones() => this._Forward.Keys;
    public IEnumerable<TTwo> Twos() => this._Backward.Keys;
}