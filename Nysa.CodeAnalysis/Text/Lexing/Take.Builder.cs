﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text.Lexing
{

    public static partial class Take
    {

        public partial class Builder
        {

            // instance members
            public Boolean IgnoreCase { get; private set; }

            private Dictionary<Char, CharacterNode> _CharacterNodes;
            private List<Take.Node>                 _Others;

            public Builder(Boolean ignoreCase)
            {
                this.IgnoreCase         = ignoreCase;
                this._CharacterNodes    = new Dictionary<Char, CharacterNode>();
                this._Others            = new List<Take.Node>();
            }

            public void Add(String literal, Identifier id)
            {
                if (String.IsNullOrWhiteSpace(literal))
                    return;

                var first = this.IgnoreCase ? Char.ToUpperInvariant(literal[0]) : literal[0];

                if (!this._CharacterNodes.ContainsKey(first))
                    this._CharacterNodes.Add(first, new CharacterNode(literal, id, this.IgnoreCase));
                else
                    this._CharacterNodes[first].Add(literal, id);
            }

            public void Add(Take.Node other)
            {
                this._Others.Add(other);
            }

            public Take.Node ToTake()
            {
                var nexts   = this._CharacterNodes.Select(n => n.Value.ToTake()).ToArray();
                // anyOne will be valid if more than one nexts is a Take.OneNode

                var anyOne  = nexts.Select(n => n as Take.OneNode)
                                   .Aggregate(String.Empty, (s, o) => o == null ? s : String.Concat(s, o.Value))
                                   .Make(any => any.Length > 1 ? any.AnyOne(this.IgnoreCase) : null);
                // longest will be valid if we have more than one valid nexts
                var others  = nexts.Where(n => !(n is Take.OneNode)).ToList();
                var longest = anyOne == null
                              ? others.Count > 1 ? Take.Longest(others) : null
                              : others.Count > 0 ? Take.Longest(others, anyOne) : null;

//                              ? Take.Longest(nexts.Where(n => !(n is Take.OneNode)).Concat(this._Others).Select(n => n.Some()), anyOne)
//                              : Take.Longest(nexts.Select(n => n.Some()).Concat(this._Others.Select(o => o.Some())));

                return   longest != null ? longest
                       : anyOne  != null ? anyOne
                       : throw new Exception("Take.Builder has no content to build with.");
            }

        }

    }

}
