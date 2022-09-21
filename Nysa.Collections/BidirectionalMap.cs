using System;
using System.Collections.Generic;

namespace Nysa.Collections
{

    public class BidirectionalMap<TAlpha, TOmega>
        where TAlpha : IEquatable<TAlpha>
        where TOmega : IEquatable<TOmega>
    {
        private Dictionary<TAlpha, TOmega> _Alphas;
        private Dictionary<TOmega, TAlpha> _Omegas;

        public BidirectionalMap(IEqualityComparer<TAlpha> alphaComparer, IEqualityComparer<TOmega> omegaComparer)
        {
            this._Alphas = new Dictionary<TAlpha, TOmega>(alphaComparer);
            this._Omegas = new Dictionary<TOmega, TAlpha>(omegaComparer);
        }

        public void Add(TAlpha alpha, TOmega omega)
        {
            this._Alphas.Add(alpha, omega);
            this._Omegas.Add(omega, alpha);
        }

        public Int32 Count => this._Alphas.Count;

        public Boolean ContainsAlpha(TAlpha one) => this._Alphas.ContainsKey(one);
        public Boolean ContainsOmega(TOmega two) => this._Omegas.ContainsKey(two);

        public TOmega GetOmega(TAlpha alpha) => this._Alphas[alpha];
        public TAlpha GetAlpha(TOmega omega) => this._Omegas[omega];

        public IEnumerable<TAlpha> Alphas() => this._Alphas.Keys;
        public IEnumerable<TOmega> Omegas() => this._Omegas.Keys;
    }

}