using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public static class NodeExtensions
    {
        public static IEnumerable<NodeOrToken> AllNotNull(this IEnumerable<NodeOrToken?> @this)
        {
            foreach (var nort in @this)
                if (nort != null)
                    yield return nort.Value;
        }

        private static IEnumerable<Node> NodesOnly(this IEnumerable<NodeOrToken> @this)
        {
            foreach (var item in @this)
                if (item.AsNode != null)
                    yield return item.AsNode;
        }

        private static IEnumerable<Node> AllNodes(this IEnumerable<Node?> @this)
        {
            foreach (var item in @this)
                if (item != null)
                    yield return item;
        }

        public static Option<Node> FirstNode(this Node node, Func<Node, Boolean> predicate)
        {
            if (predicate(node))
                return node.Some();

            foreach (var item in node.Members.Select(m => m.AsNode).AllNodes())
            {
                if (predicate(item))
                    return item.Some();

                var check = item.FirstNode(predicate);

                if (check is Some<Node>)
                    return check;
            }

            return Option<Node>.None;
        }

        public static Option<NodeOrToken> First(this Node node, Func<NodeOrToken, Boolean> predicate)
        {
            if (predicate(node))
                return ((NodeOrToken)node).Some();

            foreach (var item in node.Members)
            {
                if (predicate(item))
                    return item.Some();

                if (item.AsNode != null)
                {
                    var check = item.AsNode.First(predicate);

                    if (check is Some<NodeOrToken>)
                        return check;
                }
            }

            return Option<NodeOrToken>.None;
        }

        public static IEnumerable<NodeOrToken> Descendents(this Node node)
            => node.Members.SelectMany(m => m.AsNode != null ? Return.Enumerable(m).Concat(m.AsNode.Descendents()) : Return.Enumerable(m));
        public static IEnumerable<NodeOrToken> DescendentsAndSelf(this Node node)
            => Return.Enumerable((NodeOrToken)node).Concat(node.Descendents());

        public static IEnumerable<Node> Nodes(this Node node, Func<Node, Boolean> predicate)
            => predicate(node) ? Return.Enumerable(node).Concat(node.Members.NodesOnly().SelectMany(n => n.Nodes(predicate)))
                               : node.Members.NodesOnly().SelectMany(n => n.Nodes(predicate));

        public static IEnumerable<NodeOrToken> Where(this Node node, Func<NodeOrToken, Boolean> predicate)
            => predicate(node) ? Return.Enumerable((NodeOrToken)node).Concat(node.Members.SelectMany(m => m.Where(predicate)))
                               : node.Members.SelectMany(m => m.Where(predicate));

        public static IEnumerable<NodeOrToken> Where(this NodeOrToken item, Func<NodeOrToken, Boolean> predicate)
            =>   item.AsNode != null ? item.AsNode.Where(predicate) // call Where above
               : predicate(item)     ? Return.Enumerable(item)
               :                       None<NodeOrToken>.Enumerable();

        public static IEnumerable<Token> OrderedTokens(this Node node)
        {
            foreach (var nort in node.Members)
            {
                if (nort.AsNode != null)
                    foreach (var token in nort.AsNode.OrderedTokens())
                        yield return token;
                else if (nort.AsToken != null)
                    yield return nort.AsToken.Value;
            }
        }

        public static IEnumerable<Token> MemberTokens(this Node node)
        {
            foreach (var nort in node.Members)
            {
                if (nort.AsToken != null)
                    yield return nort.AsToken.Value;
            }
        }

    }

}
