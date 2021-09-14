using System;

using Npgsql;

namespace Nysa.Data.SqlClient
{

    public static class SqlConnectExtensions
    {

        public static String ConnectionString(this SqlConnect @this)
        {
            var build = new NpgsqlConnectionStringBuilder();
            build.Host = @this.Host;

            if (@this.Credentials != null)
            {
                build.IntegratedSecurity = false;
                build.Username = @this.Credentials.Value.User;
                build.Password = @this.Credentials.Value.Password;
            }
            else
            {
                build.IntegratedSecurity = true;
            }

            if (@this.ApplicationName != null)
                build.ApplicationName = @this.ApplicationName;

            if (@this.Database != null)
                build.Database = @this.Database;

            return build.ToString();
        }

    }

}