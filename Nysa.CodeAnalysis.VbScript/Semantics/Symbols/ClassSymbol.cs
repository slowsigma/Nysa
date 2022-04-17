using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public sealed record ClassSymbol : HardSymbol, ISymbolScope
    {
        public IReadOnlyList<Symbol>        Members { get; private set; }
        public IDictionary<String, Symbol>  Index   { get; private set; }
        public Option<Symbol>               Default { get; private set; }

        private ClassSymbol(String name, Option<String> newName, Option<String> errMessage, IEnumerable<Symbol> members, Option<Symbol> @default, IReadOnlySet<String> tags)
            : base(name, newName, errMessage, tags)
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

        public ClassSymbol(String name, Option<String> newName, Option<String> errMessage, IEnumerable<Symbol> members, Option<Symbol> @default, params String[] tags)
            : this(name, newName, errMessage, members, @default, new HashSet<String>(tags, StringComparer.OrdinalIgnoreCase))
        {
        }

        public ClassSymbol(String name, IEnumerable<Symbol> members, params String[] tags)
            : this(name, Option.None, Option.None, members, Option.None, tags)
        {
        }

        public ClassSymbol(String name, IEnumerable<Symbol> members, Option<Symbol> @default, params String[] tags)
            : this(name, Option.None, Option.None, members, @default, tags)
        {
        }

        public ClassSymbol Renamed(String newName)
            => new ClassSymbol(this.Name, newName.Some(), Option.None, this.Members, this.Default, this.Tags);
    }

}

