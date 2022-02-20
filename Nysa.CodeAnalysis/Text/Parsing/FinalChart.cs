using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorata.Text.Parsing
{

    public sealed class FinalChart : Chart
    {

        public FinalChart(Chart chart)
            : base(chart.Grammar, chart.Length)
        {
            foreach (var position in chart.Reverse())
                foreach (var entry in position.Where(e => e.NextRuleId.IsNone))
                    this[entry.Number].AddRaw(entry.AsInverted(position));
        }

        public override string ToString()
        {
            var build = new StringBuilder();

            foreach (var position in this)
            {
                build.AppendLine($"Position: [{position.Index}]");

                foreach (var entry in position.Where(e => true))
                {
                    build.AppendLine(String.Concat("   ", entry.ToString()));
                }
            }

            return build.ToString();
        }

    }

}
