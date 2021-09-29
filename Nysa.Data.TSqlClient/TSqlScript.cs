using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Text.TSql;

namespace Nysa.Data.TSqlClient
{

    public class TSqlScript
    {
        public String Value { get; init; }

        public IEnumerable<String> Batches()
            => this.Value.TSqlBatches().Select(t => this.Value.Substring(t.Start, t.Length));

        public TSqlScript(String value) { this.Value = value; }
    }

}