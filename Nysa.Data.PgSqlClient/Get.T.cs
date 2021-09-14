using System;
using System.Collections.Generic;
using System.Text;

using Npgsql;

namespace Nysa.Data.SqlClient
{

    public delegate T Get<T>(NpgsqlDataReader reader, Int32 index);

}
