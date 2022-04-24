using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record SymbolLayer : ISymbolScope
    {
        private static IReadOnlySet<String> _NoTags = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

        public Option<SymbolLayer>          Up       { get; init; }
        public ISymbolScope                 Value    { get; init; }
        public IReadOnlyList<Symbol>        Members  => this.Value.Members;
        public IDictionary<String, Symbol>  Index    => this.Value.Index;
        public IReadOnlySet<String>         Tags     => this.Value.Tags;
        public IReadOnlySet<String>         AssignedTags(String name) => this._AssignedTags.ContainsKey(name) ? this._AssignedTags[name] : _NoTags;

        private Dictionary<String, IReadOnlySet<String>> _AssignedTags;

        public SymbolLayer(Option<SymbolLayer> up, ISymbolScope value)
        {
            this.Up             = up;
            this.Value          = value;
            this._AssignedTags  = new Dictionary<String, IReadOnlySet<String>>(StringComparer.OrdinalIgnoreCase);
        }

        public Option<T> As<T>()
            where T : ISymbolScope
            => this.Value is T asT ? asT.Some() : Option<T>.None;

        public Option<T> GetMember<T>(String name)
            where T : Symbol
            => this.Value.Member<T>(name);

        public IEnumerable<T> GetMembers<T>()
            where T : Symbol
            => this.Members.Where(m => m is T).Select(v => (T)v);

        public Option<SymbolLayer> MemberLayer(String symbolName)
            =>    this.Index.ContainsKey(symbolName)
               && this.Index[symbolName] is ISymbolScope scope
               ? (new SymbolLayer(this.Some(), scope)).Some()
               : Option<SymbolLayer>.None;

        public SymbolLayer MemberLayer(ISymbolScope member)
            => new SymbolLayer(this.Some(), member);

        public Unit AssignTag(String name, IReadOnlySet<String> tags)
        {
            if (!this._AssignedTags.ContainsKey(name))
                this._AssignedTags.Add(name, tags);
            else
                this._AssignedTags[name] = tags;

            return Unit.Value;
        }
    }

}

