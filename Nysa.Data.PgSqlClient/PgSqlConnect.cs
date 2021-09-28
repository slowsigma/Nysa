using System;

using Npgsql;

namespace Nysa.Data.PgSqlClient
{

    public class PgSqlConnect
    {
        public String Host { get; init; }
        public (String User, String Password)? Credentials { get; init; }
        public String? ApplicationName { get; init; }
        public String? Database { get; init; }

        private PgSqlConnect(String host, (String user, String password)? credentials = null, String? applicationName = null, String? database = null)
        {
            this.Host               = host;
            this.Credentials        = credentials;
            this.ApplicationName    = applicationName;
            this.Database           = database;
        }

        public PgSqlConnect(String host)
            : this(host, null, null, null)
        {
        }

        public PgSqlConnect WithCredentials(String user, String password)
            => new PgSqlConnect(this.Host, (user, password), this.ApplicationName, this.Database);

        public PgSqlConnect WithApplicationName(String applicationName)
            => new PgSqlConnect(this.Host, this.Credentials, applicationName, this.Database);

        public PgSqlConnect WithDatabase(String databaseName)
            => new PgSqlConnect(this.Host, this.Credentials, this.ApplicationName, databaseName);

        public override String ToString() => this.ConnectionString();
    }

}