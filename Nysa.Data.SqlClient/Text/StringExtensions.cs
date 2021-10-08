using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.TSql
{

    public static class StringExtensions
    {
        private static readonly StateTransitions<Int32, Char> _SqlTokenTransitions;
        private static readonly StateTransitions<Int32, Char> _SqlLineTransitions;

        static StringExtensions()
        {
            _SqlTokenTransitions = SqlTextTokens.Transitions();
            _SqlLineTransitions  = SqlTextLines.Transitions();
        }

        public static IEnumerable<(Int32 Start, Int32 Length)> TSqlTokens(this String source)
        {
            var previous  = SqlTextStates.WS;
            var blocks    = (Int32)0;
            var start     = (Int32)0;
            var position  = (Int32)0;

            var machine = new StateMachine<Int32, Char>(_SqlTokenTransitions, SqlTextChars.Null, previous);

            foreach (Char value in source)
            {
                machine.Change(value);

                if (machine.State == SqlTextStates.YR)
                {
                    yield return (start, (position - start));
                    start = position;
                    machine.State = SqlTextStates.WS;
                    machine.Change(value);
                }
                else if (previous == SqlTextStates.WS)
                {
                    start = position;
                }

                if (machine.State == SqlTextStates.YC)
                {
                    yield return (start, ((position + 1) - start));
                    start = (position + 1);
                    machine.State = SqlTextStates.WS;
                }
                else if (machine.State == SqlTextStates.BC)
                {
                    blocks = blocks + ((value == SqlTextChars.Asterisk) ? 1 : -1);

                    if (blocks == 0)
                    {
                        yield return (start, ((position + 1) - start));
                        start = (position + 1);
                        machine.State = SqlTextStates.WS;
                    }
                    else
                    {
                        machine.State = SqlTextStates.BX;
                    }
                }

                position++;
                previous = machine.State;
            }

            if ((machine.State > SqlTextStates.WS) && (start < position))
            {
                yield return (start, (position - start));
            }
        }

        public static IEnumerable<(Int32 Start, Int32 Length)> TSqlLines(this String source)
        {
            var previous  = SqlTextStates.WU;
            var blocks    = (Int32)0;
            var start     = (Int32)0;
            var position  = (Int32)0;
            var machine = new StateMachine<Int32, Char>(_SqlLineTransitions, SqlTextChars.Null, previous);

            foreach (Char value in (String)source)
            {
                machine.Change(value);

                if (machine.State == SqlTextStates.RC)
                {
                    machine.State = SqlTextStates.WU;
                    machine.Change(value);
                }

                if (machine.State == SqlTextStates.YR)
                {
                    yield return (start, (position - start));
                    start = position;
                    machine.State = SqlTextStates.WU;
                    machine.Change(value);
                }

                if (machine.State == SqlTextStates.BC)
                {
                    blocks = blocks + ((value == SqlTextChars.Asterisk) ? 1 : -1);

                    machine.State = ((blocks == 0) ? SqlTextStates.WU : SqlTextStates.BX);
                }

                position++;
                previous = machine.State;
            }

            if (start < position)
            {
                yield return (start, (position - start));
            }
        }

        public static IEnumerable<(Int32 Start, Int32 Length)> TSqlBatches(this String source)
        {
            var start   = (Int32)0;
            var length  = (Int32)0;

            foreach (var (lineStart, lineLength) in source.TSqlLines())
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