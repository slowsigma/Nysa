using System;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.SqlClient
{

    public static class SqlConnectExtensions
    {

        public static String ConnectionString(this SqlConnect @this)
        {
            var build = new SqlConnectionStringBuilder();
            build.DataSource = @this.Source;

            if (@this.Credentials != null)
            {
                build.IntegratedSecurity = false;
                build.UserID = @this.Credentials.Value.Login;
                build.Password = @this.Credentials.Value.Password;
            }
            else
            {
                build.IntegratedSecurity = true;
            }

            if (@this.ApplicationName != null)
                build.ApplicationName = @this.ApplicationName;
            if (@this.Intent != null)
                build.ApplicationIntent = @this.Intent.Value;

            if (@this.InitialCatalog != null)
                build.InitialCatalog = @this.InitialCatalog;

            return build.ToString();
        }

    }

}