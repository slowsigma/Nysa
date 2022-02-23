﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Dorata.Text.Lexing
{

    public partial class Take
    {

        public partial class Builder
        {

            private class CharacterNode
            {
                public Boolean      IgnoreCase  { get; private set; }
                public Char         Value       { get; private set; }
                public Identifier?  Id          { get; private set; }
                public Dictionary<Char, CharacterNode> Nexts { get; private set; }

                public Boolean IsTerminal
                {
                    get => this.Nexts.Count == 0
                           || (this.Id == null && this.Nexts.Count == 1 && this.Nexts.First().Value.IsTerminal);
                }

                public Take.Node ToTake()
                {
                    if (this.Id == null && this.Nexts.Count == 1) // collapse
                    {
                        return this.Nexts.First().Value.ToTake(this.Value.ToString());
                    }
                    else // no collapse
                    {
                        var nexts   = this.Nexts.Select(n => n.Value.ToTake()).ToArray();
                        // anyOne will be valid if more than one nexts is a Take.OneNode
                        var anyOne  = nexts.Select(n => n as Take.OneNode).Aggregate(String.Empty, (s, o) => o == null ? s : String.Concat(s, o.Value)).AnyOne(this.IgnoreCase).Map(a => (Take.Node)a);
                        // longest will be valid if we have more than one valid nexts
                        var longest = anyOne is Some<Node> someNode
                                      ? Take.Longest(nexts.Where(n => !(n is Take.OneNode)).Select(n => n.Some()), someNode).Map(n => (Take.Node)n)
                                      : Take.Longest(nexts.Select(n => n.Some())).Map(n => (Take.Node)n);

                        var final   = longest.Or(anyOne.Or(nexts.Length > 0 ? nexts[0].Some() : Option<Node>.None));

                        if (this.Id != null)
                        {
                            // nexts take precedence over this.Id
                            final = final.Map(f => (Take.Node)f.Or(this.Id.Value.Id()))
                                         .Or(this.Id.Value.Id().Some<Node>());
                        }

                        return this.Value.One().Then(final.Value);
                    }
                }

                public Take.Node ToTake(String sequence)
                {
                    sequence = String.Concat(sequence, this.Value);

                    if (this.Id == null && this.Nexts.Count == 1) // collapse more
                    {
                        return this.Nexts.First().Value.ToTake(sequence);
                    }
                    else // cannot collapse
                    {
                        var nexts   = this.Nexts.Select(n => n.Value.ToTake()).ToArray();
                        // anyOne will be valid if more than one nexts is a Take.OneNode
                        var anyOne  = nexts.Select(n => n as Take.OneNode).Aggregate(String.Empty, (s, o) => o == null ? s : String.Concat(s, o.Value)).AnyOne(this.IgnoreCase).Map(a => (Take.Node)a);
                        // longest will be valid if we have more than one valid nexts
                        var longest = anyOne is Some<Node>
                                      ? Take.Longest(nexts.Where(n => !(n is Take.OneNode)).Select(n => n.Some()), anyOne).Map(n => (Take.Node)n)
                                      : Take.Longest(nexts.Select(n => n.Some())).Map(n => (Take.Node)n);

                        var final   = longest.Or(anyOne.Or(nexts.Length > 0 ? nexts[0].Some() : Option.None));

                        if (this.Id != null)
                        {
                            // nexts take precedence over this.Id
                            final = final.Map(f => (Take.Node)f.Or(this.Id.Value.Id()))
                                         .Or(this.Id.Value.Id().Some<Node>());
                        }

                        return sequence.Sequence(this.IgnoreCase).Then(final.Value);
                    }
                }


                public CharacterNode(String literal, Identifier id, Boolean ignoreCase)
                {
                    this.IgnoreCase = ignoreCase;
                    this.Value      = literal[0];
                    this.Nexts      = new Dictionary<Char, CharacterNode>();

                    this.Add(literal, id);
                }

                public void Add(String literal, Identifier id)
                {
                    Debug.Assert(this.IgnoreCase ? Char.ToUpperInvariant(literal[0]) == Char.ToUpperInvariant(this.Value) 
                                                 : literal[0] == this.Value
                                );

                    if (literal.Length == 1 && this.Id == null)
                        this.Id = id;
                    else if (literal.Length > 1)
                    {
                        var next = this.IgnoreCase ? Char.ToUpperInvariant(literal[1]) : literal[1];

                        if (!this.Nexts.ContainsKey(next))
                            this.Nexts.Add(next, new CharacterNode(literal.Substring(1), id, this.IgnoreCase));
                        else
                        {
                            var member = this.Nexts[next];

                            member.Add(literal.Substring(1), id);
                        }
                    }
                }

            } // class CharacterNode

        }

    }

}
