using System;
using System.Collections.Generic;

using Nysa.Logics;

using Nysa.Text.Lexing;
using ParseId = Nysa.Text.Identifier;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Until
    {
        public static Get<IEnumerable<TransformItem>> Token(ParseId parseId)
            => (b, i) =>
            {
                var r = i.Value;
                var items = new List<TransformItem>();

                for (; r < b.Length; r++)
                {
                    if (b[r] is TokenItem item && item.Value.Id.IsEqual(parseId))
                        break;
                    else
                        items.Add(b[r]);
                }

                return (items, r);
            };

        public static Get<IEnumerable<TransformItem>> NoMore()
            => (b, i) =>
            {
                var r = i.Value;
                var items = new List<TransformItem>();

                for (; r < b.Length; r++)
                {
                    items.Add(b[r]);
                }

                return (items, r);
            };
    }

}