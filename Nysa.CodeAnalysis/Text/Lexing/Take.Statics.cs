using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Dorata.Text.Lexing
{

    public static partial class Take
    {
        static Take() { Take.IgnoreCase = true; }
        public static Boolean IgnoreCase { get; set; }

        private static Node[] LongestAlternates(IEnumerable<Option<Node>> alternates)
        {
            var actuals = alternates.Where(a => a.IsSome).Select(s => s.Value);

            return actuals.Where(a => !(a is LongestNode))
                          .Concat(actuals.Where(a => a is LongestNode).SelectMany(l => ((LongestNode)l).Alternates))
                          .ToArray();
        }

        public static Option<LongestNode> Longest(IEnumerable<Option<Node>> alternatives, params Option<Node>[] additional)
        {
            var nodes = LongestAlternates(alternatives.Concat(additional));

            return (nodes.Length > 1) ? (Option<LongestNode>)new LongestNode(nodes) : Option.None;
        }

        public static Option<LongestNode> Longest(params Option<Node>[] alternatives)
        {
            var nodes = LongestAlternates(alternatives);

            return (nodes.Length > 1) ? (Option<LongestNode>)new LongestNode(nodes) : Option.None;
        }

        public static Option<AnyOneNode> AnyOne(this String alternatives, Boolean? ignoreCase = null)
        {
            var nodes = alternatives.Select(c => c.One()).ToArray();

            return (nodes.Length > 1) ? (Option<AnyOneNode>)new AnyOneNode(nodes, ignoreCase.GetValueOrDefault(Take.IgnoreCase)) : Option.None;
        }

        public static ThenNode Then(this Node primary, Node then) => new ThenNode(primary, then);
        public static ThenNode Then(this Node primary, Identifier id) => new ThenNode(primary, new IdentifierNode(id));
        public static OrNode Or(this Node primary, Node alternate) => new OrNode(primary, alternate);
        public static OneNode One(this Char value, Boolean? ignoreCase = null) => new OneNode(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
        public static SequenceNode Sequence(this String value, Boolean? ignoreCase = null) => new SequenceNode(value, ignoreCase.GetValueOrDefault(Take.IgnoreCase));
        public static IdentifierNode Id(this Identifier id) => new IdentifierNode(id);
        public static AndNode And(this Node primary, Node secondary) => new AndNode(primary, secondary);
        public static NotNode Not(this Node condition) => new NotNode(condition);
        public static MaybeNode Maybe(this Node condition) => new MaybeNode(condition);
        public static UntilNode Until(Node condition) => new UntilNode(condition);
        public static WhileNode While(this Node condition) => new WhileNode(condition);
        public static StackNode Stack(this Node push, Node pop) => new StackNode(push, pop);

        public static AssertNode Where(Func<TextSpan, Boolean> predicate)
            => new AssertNode(predicate);
        public static Node Where(this Node primary, Func<TextSpan, Boolean> predicate)
            => new ThenNode(primary, new AssertNode(predicate));

        public static SeekNode Seek(this Node subject) => new SeekNode(subject);

        public static readonly AssertNode AtStart = new AssertNode(c => c.Start.IsStart);
        public static readonly AssertNode AtEnd   = new AssertNode(c => c.End.IsEnd);

        public static IEnumerable<Hit> Repeat(this SeekNode seek, String source, Boolean includeSkips = false)
        {
            var curr = source.Start();
            var find = seek.Find(curr);
            var diff = find.Select(h => h.Span.Position - curr.End.Value, m => 0);

            while (find.IsHit)
            {
                if (diff > 0 && includeSkips)
                    yield return new Hit(new TextSpan(source, curr.End.Value, diff), Identifier.None);

                yield return find.Hit;

                curr = find.Hit.Span;
                find = seek.Find(curr.End);
            }
        }

    } // class Take

}
