using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Text.PgSql;

namespace Nysa.Data.PgSqlClient
{

    public class PgSqlScript
    {
        public String Value { get; init; }

        public IEnumerable<String> Batches()
            => this.Value.PgSqlBatches().Select(t => this.Value.Substring(t.Start, t.Length));

        public PgSqlScript(String value) { this.Value = value; }
    }

}