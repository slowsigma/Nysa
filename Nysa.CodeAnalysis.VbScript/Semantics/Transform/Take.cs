using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using ParseId = Dorata.Text.Identifier;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Take<T>
    {
        public static Get<IEnumerable<T>> ZeroOrMoreUntil(params ParseId[] parseIds)
            => (b, i) =>
            {
                var current = i;
                var items = new List<T>();

                while (current.Value < b.Length)
                {
                    if (b[current.Value] is SemanticItem semantic && semantic.Value is T node)
                    { items.Add(node); }
                    else if (b[current.Value] is TokenItem token && parseIds.Contains(token.Value.Id))
                    { break; }
                    else if (!(b[current.Value] is TokenItem))
                    { break; }

                    current = current.Value + 1;
                }

                return (items, current);
            };

    }


}