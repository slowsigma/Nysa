using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using ParseId = Dorata.Text.Identifier;
using SyntaxToken = Dorata.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Skip
    {

        // Note: Skip tokens until we find one T. If no T, then throw error.
        public static Get<T> ToExpected<T>()
            where T : CodeNode
            => (b, i) =>
            {
                var current = i;

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is SemanticItem semantic && semantic.Value is T)
                        return ((T)semantic.Value, current.Value + 1);
                    else if (b[current.Value] is TokenItem)
                        current = current.Value + 1;
                    else // cannot advance
                        break;
                }

                throw new Exception(With.MISSING_NODE);
            };

        public static Get<SyntaxToken> ToExpectedTokenOf(params ParseId[] parseIds)
            => (b, i) =>
            {
                var current = i;

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is TokenItem token && parseIds.Contains(token.Value.Id))
                        return (token.Value, current);
                    else
                        current = current.Value + 1;
                }

                throw new Exception(With.MISSING_TOKEN);
            };

        // Note: Skip tokens until we find T, then skip tokens and gather T until we run out of T.
        public static Get<IEnumerable<T>> ToZeroOrMore<T>()
            where T : CodeNode
            => (b, i) =>
            {
                var current = i;
                var items = new List<T>();

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is SemanticItem semantic && semantic.Value is T node)
                    { items.Add(node); }
                    else if (!(b[current.Value] is TokenItem))
                    { break; } // exit, current is not token and not T

                    current = current.Value + 1;
                }

                return (items, current);
            };

        // Note: Skip tokens until we find T, then skip tokens and gather T until we either run out of T or not andTrue.
        public static Get<IEnumerable<T>> ToZeroOrMore<T>(Func<T, Boolean> andTrue)
            where T : CodeNode
            => (b, i) =>
            {
                var current = i;
                var items = new List<T>();

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is SemanticItem semantic && semantic.Value is T node && andTrue(node))
                    { items.Add(node); }
                    else if (!(b[current.Value] is TokenItem))
                    { break; } // exist, current is not token or not T or not andTrue

                    current = current.Value + 1;
                }

                return (items, current);
            };

        // Note: Skip tokens until we find a node. If the node is T then return it and the advanced position.
        //       If the node is not T then return an Option<T>.None with the position at the non-T node.
        public static (Option<T> Item, Index Remainder) ToZeroOrOne<T>(this TransformItem[] @this, Index start)
            where T : CodeNode
        {
            var current = start;

            while (current.Value < @this.Length)
            {
                if (@this[current.Value] is SemanticItem semantic && semantic.Value is T node)
                    return (node.Some(), current.Value + 1);
                else if (@this[current.Value] is TokenItem)
                    current = current.Value + 1;
                else // cannot advance
                    break;
            }

            return (Option<T>.None, start);
        }

        // Note: Skip tokens until we find T, then skip tokens and gather T until we run out of T. Throw error if no T found.
        public static Get<IEnumerable<T>> ToOneOrMore<T>()
            where T : CodeNode
            => (b, i) =>
            {
                var current = i;
                var items = new List<T>();

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is SemanticItem semantic && semantic.Value is T node)
                    { items.Add(node); }
                    else if (!(b[current.Value] is TokenItem))
                    { break; }

                    current = current.Value + 1;
                }

                if (items.Count == 0)
                    throw new Exception(With.MISSING_NODE);

                return (items, current);
            };

    }

}