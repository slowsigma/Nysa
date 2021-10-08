using System;
using System.Collections.Generic;
using System.Data;

namespace Nysa.Data.TSqlClient
{

    public record TSqlParameter(String Name, Object? Value, SqlDbType sqlType);

}