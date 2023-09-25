using System;
using System.Xml.Linq;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.TSql;

public class ParseException : Exception
{
    public int Number { get; init; }
    public int Offset { get; init; }
    public int Line { get; init; }
    public int Column { get; init; }

    public ParseException(ParseError err)
        : base(err.Message)
    {
        this.Number = err.Number;
        this.Offset = err.Offset;
        this.Line = err.Line;
        this.Column = err.Column;
    }

    public override String ToString()
        => String.Concat("{", $"\"number\": {this.Number}, \"offset\": {this.Offset}, \"line\": {this.Line}, \"column\": {this.Column}, \"message\": \"{this.Message.Replace("\"", "\\\"")}\" ", "}");
}