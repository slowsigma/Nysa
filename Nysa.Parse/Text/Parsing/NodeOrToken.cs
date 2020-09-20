using System;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public struct NodeOrToken : IEquatable<NodeOrToken>, IEquatable<Node>, IEquatable<Token>
    {
        public static implicit operator NodeOrToken(Node node)   => new NodeOrToken(node);
        public static implicit operator NodeOrToken(Token token) => new NodeOrToken(token);

        public Boolean IsNode { get; private set; }
        public Boolean IsToken => !this.IsNode;

        public Node AsNode { get; private set; }
        public Token AsToken { get; private set; }

        private NodeOrToken(Node node)
        {
            this.IsNode = true;
            this.AsNode = node;
            this.AsToken = default(Token);
        }
        private NodeOrToken(Token token)
        {
            this.IsNode = false;
            this.AsNode = default(Node);
            this.AsToken = token;
        }

        public T Select<T>(Func<Node, T> fNode, Func<Token, T> fToken) => this.IsNode ? fNode(this.AsNode) : fToken(this.AsToken);

        public Boolean Equals(NodeOrToken other)
            =>   Object.ReferenceEquals(this, other)
              || (!Object.ReferenceEquals(other, null) && this.Select(a => other.IsNode && a.Equals(other.AsNode), b => other.IsToken && b.Equals(other.AsToken)));
        public Boolean Equals(Node other)
            => this.Select(a => a == other, b => false);
        public Boolean Equals(Token other)
            => this.Select(a => false, b => b.Equals(other));

        public override Boolean Equals(object obj)
            => (obj is NodeOrToken) ? this.Equals((NodeOrToken)obj) : false;

        public override Int32 GetHashCode()
            => this.Select(a => a.GetHashCode(), b => b.GetHashCode());

        public override String ToString() => base.ToString();
    }
    
}
