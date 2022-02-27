using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public static partial class VBScript
    {

        public class ParseError : Exception
        {
            public static ParseError Create(Chart chart, String source, Token[] tokens)
            {
                var errPoint = chart[0];

                while (errPoint.Index < tokens.Length - 1 && chart[errPoint.Index + 1].Count > 0)
                    errPoint = chart[errPoint.Index + 1];
                
                //var errPoint = chart.FirstOrDefault(p => p.GetEntries().Count() == 0 || p.Index == tokens.Length - 1);

                var lineStart = errPoint.Index;
                var lineStop  = errPoint.Index;

                var newLine = Grammar.Id("{new-line}");

                if (tokens[errPoint.Index].Id == newLine)
                {
                    while (lineStart > 0 && tokens[lineStart - 1].Id != newLine)
                        lineStart--;
                }
                else
                {
                    while (lineStart > 0 && tokens[lineStart - 1].Id != newLine)
                        lineStart--;

                    while (lineStop < tokens.Length && tokens[lineStop].Id != newLine)
                        lineStop++;
                }

                var lineCount = (Int32)1;
                var current   = lineStart - 1;

                while (current > -1)
                {
                    if (tokens[current].Id == newLine)
                        lineCount++;

                    current--;
                }

                var lineNumber   = lineCount;
                var columnNumber = (  tokens[errPoint.Index].Span.Position
                                     - tokens[lineStart].Span.Position     );

                var positionStart = tokens[lineStart].Span.Position;
                var positionStop  = tokens[lineStop].Span.Position + tokens[lineStop].Span.Length;

                var errorLine = positionStart >= 0 && positionStop > positionStart
                                 ? source.Substring(positionStart, (positionStop - positionStart))
                                 : String.Empty;

                if (Grammar.IsValid(tokens[errPoint.Index].Id))
                {
                    var rules = String.Join("\r\n", errPoint.Where(e => e.NextRuleId != Identifier.None && Grammar.IsTerminal(e.NextRuleId))
                                                            .Select(t => t.ToString()));

                    return new ParseError("Unexpected symbol.", lineNumber, columnNumber, errorLine, rules);
                }
                else
                {
                    return new ParseError("Invalid symbol.", lineNumber, columnNumber, errorLine, String.Empty);
                }
            }


            public Int32  LineNumber    { get; private set; }
            public Int32  ColumnNumber  { get; private set; }
            public String ErrorLine     { get; private set; }
            public String ErrorRules    { get; private set; }

            public ParseError(String message, Int32 lineNumber, Int32 columnNumber, String errorLine, String errorRules)
                : base($"Error: '{message}'; Line: {lineNumber}; Column: {columnNumber}")
            {
                this.LineNumber     = lineNumber;
                this.ColumnNumber   = columnNumber;
                this.ErrorLine      = errorLine;
                this.ErrorRules     = errorRules;
            }
        }

    }
    
}
