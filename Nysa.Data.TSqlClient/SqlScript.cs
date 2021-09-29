using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Text.TSql;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.SqlClient
{

    public class SqlScript
    {
        public String Value { get; init; }

        public IEnumerable<String> Batches()
            => this.Value.SqlBatches().Select(t => this.Value.Substring(t.Start, t.Length));

        public SqlScript(String value) { this.Value = value; }
    }

}