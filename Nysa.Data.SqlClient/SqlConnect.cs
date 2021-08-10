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

        private SqlConnect(String source, (String login, String password)? credentials = null, String? applicationName = null, ApplicationIntent? intent = null)
        {
            this.Source             = source;
            this.Credentials        = credentials;
            this.ApplicationName    = applicationName;
            this.Intent             = intent;
        }

        public SqlConnect(String source)
            : this(source, null, null, null)
        {
        }

        public SqlConnect WithCredentials(String login, String password)
            => new SqlConnect(this.Source, (login, password), this.ApplicationName, this.Intent);

        public SqlConnect WithApplicationName(String applicationName)
            => new SqlConnect(this.Source, this.Credentials, applicationName, this.Intent);

        public SqlConnect WithIntent(ApplicationIntent intent)
            => new SqlConnect(this.Source, this.Credentials, this.ApplicationName, intent);
    }

}