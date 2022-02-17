using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public interface ISymbolScope
    {
        IReadOnlyList<Symbol>       Members { get; }
        IDictionary<String, Symbol> Index   { get; }

        public Boolean Contains(String name)
            => this.Index.ContainsKey(name);
        public Option<Symbol> Member(String name)
            => this.Index.ContainsKey(name) ? this.Index[name].Some() : Option.None;
            
        public Option<T> Member<T>(String name)
            where T : Symbol
            => this.Index.ContainsKey(name)
               ? this.Index[name] is T requested
                 ? requested.Some()
                 : Option<T>.None
               : Option<T>.None;
    }

}