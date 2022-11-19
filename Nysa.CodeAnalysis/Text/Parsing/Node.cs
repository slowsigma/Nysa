using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

using Nysa.Text;

namespace Nysa.Text.Parsing
{

    public partial class Node : IEquatable<Node>
    {
        // instance members
        public Identifier Id { get; private set; }
        public String Symbol { get; private set; }

        private List<NodeOrToken> _Members;
        public IReadOnlyList<NodeOrToken> Members { get => this._Members; }

        public Option<TextSpan> First()
            => this.Members.Count == 0 ? Option<TextSpan>.None : this.Members.Select(m => m.Match(a => a.First(), b => b.Span.Some())).First();
        public Option<TextSpan> Last()
            => this.Members.Count == 0 ? Option<TextSpan>.None : this.Members.Reverse().Select(m => m.Match(a => a.Last(), b => b.Span.Some())).First();

        public Node(Identifier id, String symbol) : this(id, symbol, None<NodeOrToken>.Enumerable()) { }

        public Node(Identifier id, String symbol, IEnumerable<NodeOrToken?> members)
        {
            this.Id         = id;
            this.Symbol     = symbol;
            this._Members   = members.AllNotNull().ToList();
        }

        public Node(Identifier id, String symbol, IEnumerable<NodeOrToken> members)
        {
            this.Id         = id;
            this.Symbol     = symbol;
            this._Members   = members.ToList();
        }

        public Boolean Equals(Node? other)
            =>    !Object.ReferenceEquals(other, null)
               && this.Id == other.Id
               && this._Members.Count == other._Members.Count
               && this._Members.Zip(other.Members, (f, s) => f.Equals(s)).All(t => t);

        public override Int32 GetHashCode()
            => this.Id.HashWithOther(this._Members.HashAll());

        public override string ToString() => $"<{this.Symbol}>";

    }

}
