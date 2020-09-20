using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing
{

    public class ParseError : ParseResult
    {
        public Chart                            Chart               { get; private set; }
        public String                           SourceIdentifier    { get; private set; }
        public ParseErrorTypes                  Type                { get; private set; }
        public Int32                            LineNumber          { get; private set; }
        public Int32                            ColumnNumber        { get; private set; }
        public String                           ErrorLine           { get; private set; }
        public Option<IReadOnlyList<String>>    ErrorRules          { get; private set; }

        public ParseError(Chart chart, String source, String sourceIdentifier, Identifier newLineId, Token[] tokens)
        {
            this.Chart = chart;

            var errPoint = chart[0];

            while (errPoint.Index < tokens.Length - 1 && chart[errPoint.Index + 1].Count > 0)
                errPoint = chart[errPoint.Index + 1];

            var lineStart = errPoint.Index;
            var lineStop  = errPoint.Index;

            if (tokens[errPoint.Index].Id == newLineId)
            {
                while (lineStart > 0 && tokens[lineStart - 1].Id != newLineId)
                    lineStart--;
            }
            else
            {
                while (lineStart > 0 && tokens[lineStart - 1].Id != newLineId)
                    lineStart--;

                while (lineStop <= tokens.Length && tokens[lineStop + 1].Id != newLineId)
                    lineStop++;
            }

            var lineCount = (Int32)1;
            var current   = lineStart - 1;

            while (current > -1)
            {
                if (tokens[current].Id == newLineId)
                    lineCount++;

                current--;
            }

            this.SourceIdentifier = sourceIdentifier;

            this.LineNumber   = lineCount;
            this.ColumnNumber = (  tokens[errPoint.Index].Span.Position
                                 - tokens[lineStart].Span.Position     );

            var positionStart = tokens[lineStart].Span.Position;
            var positionStop  = tokens[lineStop].Span.Position + tokens[lineStop].Span.Length;

            this.ErrorLine = positionStart >= 0 && positionStop > positionStart
                                ? source.Substring(positionStart, (positionStop - positionStart))
                                : String.Empty;

            if (chart.Grammar.IsValid(tokens[errPoint.Index].Id))
            {
                this.Type = ParseErrorTypes.UnexpectedSymbol;

                this.ErrorRules = errPoint.Where(e => e.NextRuleId != Identifier.None && chart.Grammar.IsTerminal(e.NextRuleId))
                                          .Select(t => t.ToString())
                                          .ToList()
                                          .Some<IReadOnlyList<String>>();
            }
            else
            {
                this.Type = ParseErrorTypes.InvalidSymbol;
                this.ErrorRules = Option.None;
            }
        }
    }

}
