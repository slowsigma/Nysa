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

        public static Option<Node> FirstNode(this Node node, Func<Node, Boolean> predicate)
        {
            if (predicate(node))
                return node.Some();

            foreach (var item in node.Members.Where(m => m.IsNode).Select(x => x.AsNode))
            {
                if (predicate(item))
                    return item.Some();

                var check = item.FirstNode(predicate);

                if (check is Some<Node>)
                    return check;
            }

            return Option.None;
        }

        public static Option<NodeOrToken> First(this Node node, Func<NodeOrToken, Boolean> predicate)
        {
            if (predicate(node))
                return ((NodeOrToken)node).Some();

            foreach (var item in node.Members)
            {
                if (predicate(item))
                    return item.Some();

                if (item.IsNode)
                {
                    var check = item.AsNode.First(predicate);

                    if (check is Some<NodeOrToken>)
                        return check;
                }
            }

            return Option.None;
        }

        public static IEnumerable<NodeOrToken> Descendents(this Node node)
            => node.Members.SelectMany(m => m.IsNode ? m.Enumerable().Concat(m.AsNode.Descendents()) : m.Enumerable());
        public static IEnumerable<NodeOrToken> DescendentsAndSelf(this Node node)
            => ((NodeOrToken)node).Enumerable().Concat(node.Descendents());

        public static IEnumerable<Node> Nodes(this Node node, Func<Node, Boolean> predicate)
            => predicate(node) ? node.Enumerable().Concat(node.Members.Where(m => m.IsNode).SelectMany(n => n.AsNode.Nodes(predicate)))
                               : node.Members.Where(m => m.IsNode).SelectMany(n => n.AsNode.Nodes(predicate));

        public static IEnumerable<NodeOrToken> Where(this Node node, Func<NodeOrToken, Boolean> predicate)
            => predicate(node) ? ((NodeOrToken)node).Enumerable().Concat(node.Members.SelectMany(m => m.Where(predicate)))
                               : node.Members.SelectMany(m => m.Where(predicate));

        public static IEnumerable<NodeOrToken> Where(this NodeOrToken item, Func<NodeOrToken, Boolean> predicate)
            => item.IsNode ? item.AsNode.Where(predicate)
                           : predicate(item) ? item.Enumerable()
                                             : Enumerable.Empty<NodeOrToken>();

        public static IEnumerable<Token> Tokens(this Node node)
            => node.Members.SelectMany(m => m.IsNode ? m.AsNode.Tokens() : m.AsToken.Enumerable());

    }

}
