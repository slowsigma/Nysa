using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Nysa.Data.SqlClient
{

    public static class Of
    {
        public static Get<Boolean?>         Boolean         = new Get<Boolean?>         ((r, i) => r.IsDBNull(i) ? (Boolean?)null        : r.GetBoolean(i));
        public static Get<Byte?>            Byte            = new Get<Byte?>            ((r, i) => r.IsDBNull(i) ? (Byte?)null           : r.GetByte(i));
        public static Get<Char?>            Char            = new Get<Char?>            ((r, i) => r.IsDBNull(i) ? (Char?)null           : r.GetChar(i));
        public static Get<DateTime?>        DateTime        = new Get<DateTime?>        ((r, i) => r.IsDBNull(i) ? (DateTime?)null       : r.GetDateTime(i));
        public static Get<DateTimeOffset?>  DateTimeOffset  = new Get<DateTimeOffset?>  ((r, i) => r.IsDBNull(i) ? (DateTimeOffset?)null : r.GetDateTimeOffset(i));
        public static Get<Int64?>           Int64           = new Get<Int64?>           ((r, i) => r.IsDBNull(i) ? (Int64?)null          : r.GetInt64(i));
        public static Get<Int32?>           Int32           = new Get<Int32?>           ((r, i) => r.IsDBNull(i) ? (Int32?)null          : r.GetInt32(i));
        public static Get<Int16?>           Int16           = new Get<Int16?>           ((r, i) => r.IsDBNull(i) ? (Int16?)null          : r.GetInt16(i));
        public static Get<String>           String          = new Get<String>           ((r, i) => r.GetString(i));
        public static Get<TimeSpan?>        TimeSpan        = new Get<TimeSpan?>        ((r, i) => r.IsDBNull(i) ? (TimeSpan?)null       : r.GetTimeSpan(i));
        public static Get<Guid?>            Guid            = new Get<Guid?>            ((r, i) => r.IsDBNull(i) ? (Guid?)null           : r.GetGuid(i));
        public static Get<Double?>          Double          = new Get<Double?>          ((r, i) => r.IsDBNull(i) ? (Double?)null         : r.GetDouble(i));
        public static Get<Single?>          Single          = new Get<Single?>          ((r, i) => r.IsDBNull(i) ? (Single?)null         : r.GetFloat(i));
        public static Get<Decimal?>         Decimal         = new Get<Decimal?>         ((r, i) => r.IsDBNull(i) ? (Decimal?)null        : r.GetDecimal(i));
    }

}
