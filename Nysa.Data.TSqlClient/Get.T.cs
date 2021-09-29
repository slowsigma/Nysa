using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace Nysa.Data.TSqlClient
{

    public delegate T Get<T>(SqlDataReader reader, Int32 index);

}
