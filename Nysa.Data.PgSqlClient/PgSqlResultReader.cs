using System;

using Npgsql;

namespace Nysa.Data
{

    public class PgSqlResultReader : IDisposable
    {
        public Int32 ResultIndex { get; private set; }
        public Int32 RowIndex    { get; private set; }

        public PgSqlResultReader(NpgsqlDataReader sqlDataReader)
        {
            this.ResultIndex    = -1;
            this.RowIndex       = -1;
            this._RowReader     = sqlDataReader;
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
                return this._RowReader != null && this._RowReader.NextResult();
            }
        }

        public Boolean ReadRow()
        {
            return this._RowReader != null && this._RowReader.Read();
        }

        private NpgsqlDataReader? _RowReader;
        public NpgsqlDataReader Row => this._RowReader!;

        public void Dispose()
        {
            if (this._RowReader != null && !this._RowReader.IsClosed)
            {
                this._RowReader.Close();
                ((IDisposable)this._RowReader).Dispose();
                this._RowReader = null;
            }
        }
    }

}
