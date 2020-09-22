using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.Parsing
{

    public class Node : IEquatable<Node>
    {
        public Identifier                   Id      { get; private set; }
        public String                       Symbol  => this._Symbol.Value;
        public IReadOnlyList<NodeOrToken>   Members => this._Members;

        private Lazy<String>        _Symbol;
        private List<NodeOrToken>   _Members;

        public Option<TextSpan> First()
            => this.Members.Count == 0 ? Option<TextSpan>.None : this.Members.Select(m => m.Select(a => a.First(), b => b.Span.Some())).First();
        public Option<TextSpan> Last()
            => this.Members.Count == 0 ? Option<TextSpan>.None : this.Members.Reverse().Select(m => m.Select(a => a.Last(), b => b.Span.Some())).First();

        public Node(Identifier id, Lazy<String> symbol) : this(id, symbol, Enumerable.Empty<NodeOrToken>()) { }

        public Node(Identifier id, Lazy<String> symbol, IEnumerable<NodeOrToken> members)
        {
            this.Id         = id;
            this._Symbol    = symbol;
            this._Members   = members.ToList();
        }

        public Boolean Equals(Node other)
            =>    !Object.ReferenceEquals(other, null)
               && this.Id == other.Id
               && this._Members.Count == other._Members.Count
               && this._Members.Zip(other.Members, (f, s) => f.Equals(s)).All(t => t);

        public override Int32 GetHashCode() => this.Id.HashWithOther(this._Members.HashAll());
        public override string ToString() => $"<{this.Symbol}>";
    }

}
