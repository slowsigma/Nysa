using System;
using System.Data.SqlClient;

namespace Nysa.Data
{

    public class SqlResultReader : IDisposable
    {
        public Int32 ResultIndex { get; private set; }
        public Int32 RowIndex    { get; private set; }

        public SqlResultReader(SqlDataReader sqlDataReader)
        {
            this.ResultIndex    = -1;
            this.RowIndex       = -1;
            this.RowReader      = sqlDataReader;
        }

        public Boolean ReadResult()
        {
            if (this.ResultIndex < 0)
            {
                this.ResultIndex = 0;
                return true;
            }
            else
            {
                this.ResultIndex++;
                this.RowIndex = -1;
                return this.RowReader.NextResult();
            }
        }

        public Boolean ReadRow()
        {
            return this.RowReader.Read();
        }

        private SqlDataReader RowReader;
        public SqlDataReader Row => this.RowReader;

        public void Dispose()
        {
            if (this.RowReader != null && !this.RowReader.IsClosed)
            {
                this.RowReader.Close();
                ((IDisposable)this.RowReader).Dispose();
                this.RowReader = null;
            }
        }
    }

}
