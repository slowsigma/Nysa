using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public class ParseException : Exception
    {
        public Int32  LineNumber    { get; private set; }
        public Int32  ColumnNumber  { get; private set; }
        public String ErrorLine     { get; private set; }
        public String ErrorRules    { get; private set; }

        public ParseException(String message, Int32 lineNumber, Int32 columnNumber, String errorLine, String errorRules)
            : base($"Error: '{message}'; Line: {lineNumber}; Column: {columnNumber}")
        {
            this.LineNumber     = lineNumber;
            this.ColumnNumber   = columnNumber;
            this.ErrorLine      = errorLine;
            this.ErrorRules     = errorRules;
        }
    }

}