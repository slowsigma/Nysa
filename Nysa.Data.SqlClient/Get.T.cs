using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Nysa.Data.SqlClient
{

    public delegate T Get<T>(SqlDataReader reader, Int32 index);

}
