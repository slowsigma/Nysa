using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.Lexing;

internal sealed class CharacterNode
{
    public Boolean      IgnoreCase  { get; private set; }
    public Char         Value       { get; private set; }
    public Identifier?  Id          { get; private set; }
    public Dictionary<Char, CharacterNode> Nexts { get; private set; }

    private Rule GetNext()
    {
        var nextRules   = this.Nexts.Select(n => n.Value.ToRule()).ToArray();
        // start with everything that we're not turning into a set
        var nexts       = nextRules.Where(n => !(n is OneRule o && o.IgnoreCase == this.IgnoreCase)).ToList();
        // pull out ones we can make into a set
        var oneRules    = nextRules.Where(n => n is OneRule o && o.IgnoreCase == this.IgnoreCase).Cast<OneRule>().ToArray();
        // if have enough for a set
        if (oneRules.Length > 1) // create it and add to nexts
            nexts.Add(oneRules.Aggregate(String.Empty, (s, o) => String.Concat(s, o.Value)).Set(this.IgnoreCase));
        else // otherwise, put what we can't use back into nexts
            nexts.AddRange(oneRules);
        // if we have more than one, we turn nexts into a single LongestRule
        if (nexts.Count > 1)
            nexts = Return.Enumerable(Find.Longest(nexts)).ToList<Rule>();

        return   nexts.Count == 1 && this.Id != null ? nexts[0].Or(this.Id.Value.IdX())
               : nexts.Count == 1                    ? nexts[0]
               :                     this.Id != null ? this.Id.Value.IdX()
               : throw new Exception("Program error.");
    }

    public Rule ToRule()
        => (this.Id == null && this.Nexts.Count == 1)
           ? this.Nexts.First().Value.ToRule(this.Value.ToString())     // collapse more
           : this.Value.One().Then(this.GetNext());                    // no collapse

    public Rule ToRule(String sequence)
        => (this.Id == null && this.Nexts.Count == 1)
           ? this.Nexts.First().Value.ToRule(String.Concat(sequence, this.Value))                   // collapse more
           : String.Concat(sequence, this.Value).Sequence(this.IgnoreCase).Then(this.GetNext());   // no collapse

    public CharacterNode(String literal, Identifier id, Boolean ignoreCase)
    {
        this.IgnoreCase = ignoreCase;
        this.Value      = literal[0];
        this.Nexts      = new Dictionary<Char, CharacterNode>();

        this.Add(literal, id);
    }

    public void Add(String literal, Identifier id)
    {
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
  

}