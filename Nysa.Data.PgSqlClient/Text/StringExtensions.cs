using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.PgSql
{

    public static class StringExtensions
    {
        private static readonly StateTransitions<PgSqlState, Char> _PgSqlLineTransitions;

        static StringExtensions()
        {
            _PgSqlLineTransitions = Nysa.Text.PgSql.PgSqlLines.Transitions();
        }


        public static IEnumerable<(Int32 Start, Int32 Length)> PgSqlLines(this String source)
        {
            var previous  = PgSqlState.WU;
            var blocks    = (Int32)0;
            var start     = (Int32)0;
            var position  = (Int32)0;
            var machine = new StateMachine<PgSqlState, Char>(_PgSqlLineTransitions, PgSqlChars.Null, previous);

            foreach (Char value in (String)source)
            {
                machine.Change(value);

                if (machine.State.Equals(PgSqlState.RC)) // reset & recheck
                {
                    machine.State = PgSqlState.WU;
                    machine.Change(value);
                }

                if (machine.State.Equals(PgSqlState.YR)) // yield result
                {
                    yield return (start, (position - start));
                    start = position;
                    machine.State = PgSqlState.WU;
                    machine.Change(value);
                }

                if (machine.State.Equals(PgSqlState.BC)) // block comment counting
                {
                    blocks = blocks + ((value == PgSqlChars.Asterisk) ? 1 : -1);

                    machine.State = ((blocks == 0) ? PgSqlState.WU : PgSqlState.BX);
                }

                position++;
                previous = machine.State;
            }

            if (start < position)
            {
                yield return (start, (position - start));
            }
        }
        
        public static IEnumerable<(Int32 Start, Int32 Length)> PgSqlBatches(this String source)
        {
            var start   = (Int32)0;
            var length  = (Int32)0;

            foreach (var (lineStart, lineLength) in source.PgSqlLines())
            {
                if (   (lineLength >= 2)
                    && (source[lineStart] == 'g' || source[lineStart] == 'G')
                    && (source[lineStart + 1] == 'o' || source[lineStart + 1] == 'O')
                    && ((lineLength == 2) || Char.IsWhiteSpace(source[lineStart + 2])))
                {
                    yield return (start, length);           // do not include the "GO" line

                    start  = (start + length + lineLength); // the new start is after the "GO" line
                    length = 0;
                }
                else
                {
                    length += lineLength;
                }
            }

            if (length > 0)
                yield return (start, length);
        }


    }

}