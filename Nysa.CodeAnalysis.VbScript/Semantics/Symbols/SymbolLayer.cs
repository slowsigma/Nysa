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

        private Dictionary<String, HashSet<String>> _AssignedTags;

        public SymbolLayer(Option<SymbolLayer> up, ISymbolScope value)
        {
            this.Up             = up;
            this.Value          = value;
            this._AssignedTags  = new Dictionary<String, HashSet<String>>(StringComparer.OrdinalIgnoreCase);
        }

        public Option<SymbolLayer> MemberLayer(String symbolName)
            =>    this.Index.ContainsKey(symbolName)
               && this.Index[symbolName] is ISymbolScope scope
               ? (new SymbolLayer(this.Some(), scope)).Some()
               : Option<SymbolLayer>.None;

        public SymbolLayer MemberLayer(ISymbolScope member)
            => new SymbolLayer(this.Some(), member);

        public Unit AssignTag(String name, String tag)
        {
            if (!this._AssignedTags.ContainsKey(name))
                this._AssignedTags.Add(name, new HashSet<String>(StringComparer.OrdinalIgnoreCase));

            this._AssignedTags[name].Add(tag);

            return Unit.Value;
        }
    }

}

