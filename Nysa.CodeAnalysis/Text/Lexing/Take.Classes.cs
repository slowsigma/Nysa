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

        /// <summary>
        /// Defines the basic functionality any Take.Node must have.
        /// </summary>
        public abstract class Node
        {
            public abstract HitOrMiss Find(TextSpan current);
        }

        public class IdentifierNode : Node
        {
            public Identifier Id { get; private set; }
            public IdentifierNode(Identifier id) { this.Id = id; }
            public override HitOrMiss Find(TextSpan current)
                => new Hit(current, this.Id);
            public override string ToString()
                => $"{nameof(IdentifierNode)} {this.Id}";
        }

        public class ThenNode : Node
        {
            public Node Primary { get; private set; }
            public Node Then { get; private set; }
            public ThenNode(Node primary, Node then) { this.Primary = primary; this.Then = then; }
            public override HitOrMiss Find(TextSpan current)
                => this.Primary
                       .Find(current)
                       .Select(hp => this.Then.Find(hp.Span)
                                              .Select(ht => ht, mt => HitOrMiss.NewMiss(mt.Size + (hp.Span.Length - current.Length))),
                               mp => mp);

            public override string ToString()
                => $"{nameof(ThenNode)} {this.Primary} -> {this.Then}";
        }

        public class AssertNode : Node
        {
            private Func<TextSpan, Boolean> _Predicate;
            public AssertNode(Func<TextSpan, Boolean> predicate) { this._Predicate = predicate; }
            public override HitOrMiss Find(TextSpan current)
                => this._Predicate(current) ? HitOrMiss.NewHit(current) : new Miss(0);
        }

        public class StartNode : AssertNode
        {
            public StartNode() : base(c => c.Start.IsStart) { }
            public override string ToString()
                => $"{nameof(StartNode)}";
        }

        public class EndNode : AssertNode
        {
            public EndNode() : base(c => c.End.IsEnd) { }
            public override string ToString()
                => $"{nameof(EndNode)}";
        }

        public class ExpandNode : Node
        {
            private Func<TextSpan, HitOrMiss> _Expander;
            public ExpandNode(Func<TextSpan, HitOrMiss> expander) { this._Expander = expander; }
            public override HitOrMiss Find(TextSpan current)
                => this._Expander(current);
        }

        public class OneNode : ExpandNode
        {
            public Char     Value       { get; private set; }
            public Boolean  IgnoreCase  { get; private set; }

            public OneNode(Char value, Boolean ignoreCase)
                : base(c => c.End.Next().Select(n => OneEquals(value, n, ignoreCase) ? HitOrMiss.NewHit(c.Appended(1)) : HitOrMiss.NewMiss(1)).OrValue(HitOrMiss.NewMiss(1)))
            {
                this.Value      = value;
                this.IgnoreCase = ignoreCase;
            }

            private static Boolean OneEquals(Char one, Char other, Boolean ignoreCase)
                => ignoreCase
                   ? Char.ToUpperInvariant(one).Equals(Char.ToUpperInvariant(other))
                   : one.Equals(other);

            public override String ToString()
                => $"{nameof(OneNode)} {this.Value}";
        }

        public class SequenceNode : ExpandNode
        {
            public String   Value       { get; private set; }
            public Boolean  IgnoreCase  { get; private set; }

            public SequenceNode(String value, Boolean ignoreCase)
                : base(c => SequenceEquals(value, c.End.Next(value.Length), ignoreCase) ? HitOrMiss.NewHit(c.Appended(value.Length)) : HitOrMiss.NewMiss(value.Length))
            {
                this.Value      = value;
                this.IgnoreCase = ignoreCase;
            }

            private static Boolean SequenceEquals(String sequence, String other, Boolean ignoreCase)
                => ignoreCase
                   ? sequence.ToUpperInvariant().Equals(other.ToUpperInvariant())
                   : sequence.Equals(other);

            public override String ToString()
                => $"{nameof(SequenceNode)} {this.Value}";
        }

        public class AnyOneNode : Node
        {
            public String   Alternatives    { get; private set; }
            public Boolean  IgnoreCase      { get; private set; }

            private HashSet<Char> _Alternatives;

            public AnyOneNode(String alternatives, Boolean ignoreCase)
            {
                this.Alternatives   = alternatives;
                this.IgnoreCase     = ignoreCase;
                this._Alternatives  = new HashSet<Char>(this.Alternatives.Select(c => this.IgnoreCase ? Char.ToUpperInvariant(c) : c).Distinct());
            }
            public AnyOneNode(IEnumerable<OneNode> alternatives, Boolean ignoreCase)
                : this(alternatives.Aggregate(String.Empty, (s, n) => String.Concat(s, n.Value)), ignoreCase)
            {
            }

            public override HitOrMiss Find(TextSpan current)
                => current.End
                          .Next()
                          .Select(n => this._Alternatives.Contains(this.IgnoreCase ? Char.ToUpperInvariant(n) : n) ? HitOrMiss.NewHit(current.Appended(1)) : HitOrMiss.NewMiss(1))
                          .OrValue(HitOrMiss.NewMiss(1));

            public override String ToString()
                => $"{nameof(AnyOneNode)} {this.Alternatives}";
        }

        public class NotNode : Node
        {
            public Node Condition { get; private set; }
            public NotNode(Node condition) { this.Condition = condition; }
            public override HitOrMiss Find(TextSpan current)
             => this.Condition.Find(current).Select(h => HitOrMiss.NewMiss(h.Span.Length - current.Length),
                                                    m => HitOrMiss.NewHit(current.Appended(m.Size), Identifier.None));
        }

        public class OrNode : Node
        {
            public Node First  { get; private set; }
            public Node Second { get; private set; }
            public OrNode(Node first, Node second) { this.First = first; this.Second = second; }
            public override HitOrMiss Find(TextSpan current)
                => this.First.Find(current).Select(h => h, m => this.Second.Find(current));
        }

        public class AndNode : Node
        {
            public Node First   { get; private set; }
            public Node Second  { get; private set; }
            public AndNode(Node first, Node second) { this.First = first; this.Second = second; }
            public override HitOrMiss Find(TextSpan current)
            {
                var f = this.First.Find(current);
                var s = this.Second.Find(current);

                if (f.IsHit && s.IsHit) //success
                {
                    var largestHit = HitOrMiss.Largest(f.ToHit(), s.ToHit());
                    // identifiedHit is not guaranteed to have value
                    var identifiedHit = HitOrMiss.Identified(f.ToHit(), s.ToHit());

                    return HitOrMiss.NewHit(largestHit.Value.Span, identifiedHit.IsSome ? identifiedHit.Value.Id : Identifier.None);
                }
                else if (f.IsHit)
                    return HitOrMiss.Smallest(new Miss(f.Hit.Span.Length - current.Length), s.Miss);
                else if (s.IsHit)
                    return HitOrMiss.Smallest(f.ToMiss(), new Miss(s.Hit.Span.Length - current.Length));
                else
                    return HitOrMiss.Smallest(f.ToMiss(), s.ToMiss());
            }
        }


        public class UntilNode : ExpandNode
        {
            public UntilNode(Node condition) : base(c => TryUntil(c, condition)) { }
            private static HitOrMiss TryUntil(TextSpan current, Node condition)
            {
                var conditionTry = condition.Find(current);
                var totalMiss    = 0;

                while (conditionTry.IsMiss && !current.End.IsEnd)
                {
                    current   =  current.Appended(conditionTry.Miss.Size);
                    totalMiss += conditionTry.Miss.Size;

                    conditionTry = condition.Find(current);
                }

                return conditionTry.Select(h => HitOrMiss.NewHit(current, Identifier.None), m => HitOrMiss.NewMiss(totalMiss + m.Size));
            }
        }

        public class WhileNode : ExpandNode
        {
            public WhileNode(Node condition) : base(c => TryWhile(c, condition)) { }
            private static HitOrMiss TryWhile(TextSpan current, Node condition)
            {
                Identifier id = Identifier.None;

                while (!current.End.IsEnd)
                {
                    var find = condition.Find(current);

                    if (find.IsMiss)
                        return HitOrMiss.NewHit(current, id);

                    current = find.Hit.Span;
                    id      = find.Hit.Id;
                }

                return HitOrMiss.NewHit(current, id);
            }
        }

        public class MaybeNode : Node
        {
            public Node Condition { get; private set; }
            public MaybeNode(Node condition) { this.Condition = condition; }
            public override HitOrMiss Find(TextSpan current)
                => this.Condition.Find(current).Select(h => h, m => HitOrMiss.NewHit(current, Identifier.None));
        }

        public class LongestNode : Node
        {
            private List<Node> _Alternates;
            public IReadOnlyList<Node> Alternates { get => this._Alternates; }
            public LongestNode(IEnumerable<Node> alternates) => this._Alternates = alternates.Where(a => a != null).ToList();
            public override HitOrMiss Find(TextSpan current)
            {
                var miss    = new Miss(Int32.MaxValue);
                var hit     = new Hit(current, Identifier.None);
                var isHit   = false;

                foreach (var node in this._Alternates)
                {
                    var find = node.Find(current);

                    isHit |= find.IsHit;
                    
                    find.Apply(h => hit  = hit.Span.Value.Length < h.Span.Value.Length ? h : hit,
                               m => miss = m.Size                < miss.Size           ? m : miss );
                }

                return isHit ? hit
                             : (miss.Size == Int32.MaxValue ? HitOrMiss.NewMiss(0) : miss);
            }
        }

        public class StackNode : ExpandNode
        {
            public StackNode(Node push, Node pop) : base(c => TryExpand(c, push, pop)) { }

            private static HitOrMiss SpanPush(Hit hit, Node push, Node pop)
            {
                var popTry      = pop.Find(hit.Span);  // pop gets a chance to declare a hit
                var @continue   = !hit.Span.End.IsEnd; // even if we're at the end of the data

                while (popTry.IsMiss && @continue)
                {
                    // before we expand, check to see if we need to push again
                    var pushTry = push.Find(hit.Span);

                    // if get a hit on push, we start another SpanPush and if that
                    // does not return a hit, we have a pop did not occur and we've
                    // come to the end of the data
                    if (pushTry.IsHit)
                        SpanPush(pushTry.Hit, push, pop).Apply(h => hit = h, m => @continue = false);
                    else // no additional push for now, just expand by one
                        hit = new Hit(hit.Span.Appended(1), Identifier.None);

                    popTry      = pop.Find(hit.Span);
                    @continue   = !hit.Span.End.IsEnd;
                }

                return popTry;
            }

            private static HitOrMiss TryExpand(TextSpan span, Node push, Node pop)
            {
                var start = push.Find(span);

                return (start.IsHit) 
                       ? SpanPush(start.Hit, push, pop)
                       : HitOrMiss.NewMiss(1);
            }

        }

        public class SeekNode : Node
        {
            public Node Subject { get; private set; }
            public SeekNode(Node subject) { this.Subject = subject; }
            public override HitOrMiss Find(TextSpan current)
            {
                var check = this.Subject.Find(current);

                while (check.IsMiss && !current.End.IsEnd)
                {
                    current = current.End + 1;
                    check   = this.Subject.Find(current);
                }

                return check;
            }
        }

    } // Take

}
