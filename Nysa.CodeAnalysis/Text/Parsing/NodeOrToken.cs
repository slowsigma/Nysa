using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;
using Dorata.Text.Lexing;

namespace Dorata.Text.Parsing
{

    public struct NodeOrToken : IEquatable<NodeOrToken>, IEquatable<Node>, IEquatable<Token>
    {
        public static implicit operator NodeOrToken(Node node)   => new NodeOrToken(node);
        public static implicit operator NodeOrToken(Token token) => new NodeOrToken(token);

        public Node?   AsNode { get; private set; }
        public Token?  AsToken { get; private set; }

        private NodeOrToken(Node node)
        {
            this.AsNode  = node;
            this.AsToken = null;
        }
        private NodeOrToken(Token token)
        {
            this.AsNode  = null;
            this.AsToken = token;
        }

        public T Select<T>(Func<Node, T> fNode, Func<Token, T> fToken)
            => this.AsNode != null ? fNode(this.AsNode) : this.AsToken != null ? fToken(this.AsToken.Value) : throw new Exception("Program error.");

        public Boolean Equals(NodeOrToken other)
            =>   Object.ReferenceEquals(this, other)
              || (!Object.ReferenceEquals(other, null) && this.Select(a => other.AsNode != null && a.Equals(other.AsNode), b => other.AsToken != null && b.Equals(other.AsToken.Value)));
        public Boolean Equals(Node? other)
            => this.Select(a => a == other, b => false);
        public Boolean Equals(Token other)
            => this.Select(a => false, b => b.Equals(other));

        public override Boolean Equals(object? obj)
            => obj is NodeOrToken nort ? this.Equals(nort) : false;

        public override Int32 GetHashCode()
            => this.Select(a => a.GetHashCode(), b => b.GetHashCode());

        public override String? ToString() => base.ToString();
    }
    
}
