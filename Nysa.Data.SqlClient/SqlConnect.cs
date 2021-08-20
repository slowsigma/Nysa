using System;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.SqlClient
{

    public class SqlConnect
    {
        public String Source { get; init; }
        public (String Login, String Password)? Credentials { get; init; }
        public String? ApplicationName { get; init; }
        public ApplicationIntent? Intent { get; init; }
        public String? InitialCatalog { get; init; }

        private SqlConnect(String source, (String login, String password)? credentials = null, String? applicationName = null, ApplicationIntent? intent = null, String? initialCatalog = null)
        {
            this.Source             = source;
            this.Credentials        = credentials;
            this.ApplicationName    = applicationName;
            this.Intent             = intent;
            this.InitialCatalog     = initialCatalog;
        }

        public SqlConnect(String source)
            : this(source, null, null, null, null)
        {
        }

        public SqlConnect WithCredentials(String login, String password)
            => new SqlConnect(this.Source, (login, password), this.ApplicationName, this.Intent, this.InitialCatalog);

        public SqlConnect WithApplicationName(String applicationName)
            => new SqlConnect(this.Source, this.Credentials, applicationName, this.Intent, this.InitialCatalog);

        public SqlConnect WithIntent(ApplicationIntent intent)
            => new SqlConnect(this.Source, this.Credentials, this.ApplicationName, intent, this.InitialCatalog);

        public SqlConnect WithInitialCatalog(String databaseName)
            => new SqlConnect(this.Source, this.Credentials, this.ApplicationName, this.Intent, databaseName);
    }

}