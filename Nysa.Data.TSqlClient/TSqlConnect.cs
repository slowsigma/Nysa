using System;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.TSqlClient
{

    public class TSqlConnect
    {
        public String Source { get; init; }
        public (String Login, String Password)? Credentials { get; init; }
        public String? ApplicationName { get; init; }
        public ApplicationIntent? Intent { get; init; }
        public String? InitialCatalog { get; init; }

        private TSqlConnect(String source, (String login, String password)? credentials = null, String? applicationName = null, ApplicationIntent? intent = null, String? initialCatalog = null)
        {
            this.Source             = source;
            this.Credentials        = credentials;
            this.ApplicationName    = applicationName;
            this.Intent             = intent;
            this.InitialCatalog     = initialCatalog;
        }

        public TSqlConnect(String source)
            : this(source, null, null, null, null)
        {
        }

        public TSqlConnect WithCredentials(String login, String password)
            => new TSqlConnect(this.Source, (login, password), this.ApplicationName, this.Intent, this.InitialCatalog);

        public TSqlConnect WithApplicationName(String applicationName)
            => new TSqlConnect(this.Source, this.Credentials, applicationName, this.Intent, this.InitialCatalog);

        public TSqlConnect WithIntent(ApplicationIntent intent)
            => new TSqlConnect(this.Source, this.Credentials, this.ApplicationName, intent, this.InitialCatalog);

        public TSqlConnect WithInitialCatalog(String databaseName)
            => new TSqlConnect(this.Source, this.Credentials, this.ApplicationName, this.Intent, databaseName);

        public override String ToString() => this.ConnectionString();
    }

}