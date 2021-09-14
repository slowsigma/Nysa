using System;

using Npgsql;

namespace Nysa.Data.SqlClient
{

    public class SqlConnect
    {
        public String Host { get; init; }
        public (String User, String Password)? Credentials { get; init; }
        public String? ApplicationName { get; init; }
        public String? Database { get; init; }

        private SqlConnect(String host, (String user, String password)? credentials = null, String? applicationName = null, String? database = null)
        {
            this.Host               = host;
            this.Credentials        = credentials;
            this.ApplicationName    = applicationName;
            this.Database           = database;
        }

        public SqlConnect(String source)
            : this(source, null, null, null)
        {
        }

        public SqlConnect WithCredentials(String user, String password)
            => new SqlConnect(this.Host, (user, password), this.ApplicationName, this.Database);

        public SqlConnect WithApplicationName(String applicationName)
            => new SqlConnect(this.Host, this.Credentials, applicationName, this.Database);

        public SqlConnect WithDatabase(String databaseName)
            => new SqlConnect(this.Host, this.Credentials, this.ApplicationName, databaseName);
    }

}