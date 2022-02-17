using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public record ClassSymbol : HardSymbol, ISymbolScope
    {
        public IReadOnlyList<Symbol>       Members  { get; private set; }
        public IDictionary<String, Symbol> Index    { get; private set; }
        public Option<Symbol>              Default  { get; private set; }

        public ClassSymbol(String name, Option<String> newName, IEnumerable<Symbol> members, Option<Symbol> @default)
            : base(name, newName)
        {
            if (members.Any(s => s is ArgumentSymbol || s is ClassSymbol))
                throw new ArgumentException("ClassSymbol members can only be of type: ConstantSymbol, FunctionSymbol, VariableSymbol, or PropertySymbol.");

            if (@default is Some<Symbol> someDefault && !members.Any(s => s.Name.Equals(someDefault.Value.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("The default symbol must exist in the members collection.");

            var parts = Symbols.Distinct(members);

            this.Members = parts.Members;
            this.Index   = parts.Index;

            this.Default = @default.Map(d => this.Index[d.Name]);
        }

        public ClassSymbol(String name, IEnumerable<Symbol> members)
            : this(name, Option.None, members, Option.None)
        {
        }

        public ClassSymbol(String name, IEnumerable<Symbol> members, Option<Symbol> @default)
            : this(name, Option.None, members, @default)
        {
        }

        public ClassSymbol Renamed(String newName)
            => new ClassSymbol(this.Name, newName.Some(), this.Members, this.Default);
    }

}

