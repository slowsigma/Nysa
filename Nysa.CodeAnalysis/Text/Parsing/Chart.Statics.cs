using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public partial class Chart
    {
        // static members
        public static Chart Create(Grammar grammar, Token[] input)
        {
            var chart = new Chart(grammar, input.Length + 1);

            if (grammar.IsTerminal(grammar.StartId))
                return chart;

            foreach (var rule in grammar.Rules(grammar.StartId))
                chart[0].AddRaw(new Chart.Entry(rule, 0, 0));

            for (Int32 p = 0; p < input.Length; p++)
            {
                for (Int32 e = 0; e < chart[p].Count; e++)
                {
                    var entry       = chart[p][e];
                    var symbolId    = entry.NextRuleId;

                    if (symbolId.IsNone) // test for completion
                        foreach (var incomplete in chart[entry.Number].GetEntries().Where(i => !i.NextRuleId.IsNone && i.NextRuleId == entry.Rule.Id))
                            chart[p].AddUnique(incomplete.AsNextEntry());

                    else if (grammar.IsTerminal(symbolId)) // test for terminal
                    {
                        if (symbolId == input[p].Id)
                            chart[p + 1].AddRaw(entry.AsNextEntry());
                    }
                    else // use rules to anticipate input and completion
                    {
                        foreach (var rule in grammar.Rules(symbolId))
                        {
                            chart[p].AddUnique(rule, p, 0);

                            if (grammar.NullableIds.Contains(symbolId))
                                chart[p].AddUnique(entry.AsNextEntry());
                        }
                    }
                } // foreach entry in position
            } // foreach position in input/chart

            return chart;
        }

    }

}
