using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.Sql
{

    internal static class SqlTextLines
    {

        public static StateTransitions<Int32, Char> Transitions()
        {
            var transitions = new StateTransitions<Int32, Char>();

            // Line transitions are only those transitions necessary for determining lines in a T-SQL batch.

            // Notes On Line Transitions:
            //    1. State   0 is the basic state for white space and anything else not recognized.
            //    2. State  49 is a trapping state used to count comment blocks.  The state indicates either:
            //                 a. "/*" the block-comment counter is incremented and the state is set to 40
            //                 b. "*/" the block-comment counter is decremented and...
            //                    i. if counter is zero, the lexing logic yields and the state is set to 0, or
            //                    ii. the state is set to 40.
            //    3. State 888 tells the lexing logic to reset the current state to 0 and recheck the current character.
            //    4. State 999 tells the lexing logic to yield without the current character, reset current state to 0, and recheck the current character.
            //    5. The SqlTextChars.Null negator character is used for both a miss condition and the input termination condition.

            // white-space
            transitions.Add(SqlTextStates.WU, SqlTextChars.Space,           SqlTextStates.WU);
            transitions.Add(SqlTextStates.WU, SqlTextChars.Tab,             SqlTextStates.WU);
            transitions.Add(SqlTextStates.WU, SqlTextChars.Null,            SqlTextStates.WU);         // if the character is not "recognized" stay in zero
            // carriage-return and line-feed hold
            transitions.Add(SqlTextStates.WU, SqlTextChars.CarriageReturn,  SqlTextStates.LI);
            transitions.Add(SqlTextStates.WU, SqlTextChars.LineFeed,        SqlTextStates.LI);
            // hyphen hold
            transitions.Add(SqlTextStates.WU, SqlTextChars.HyphenMinus,     SqlTextStates.HY);         // -
            transitions.Add(SqlTextStates.HY, SqlTextChars.HyphenMinus,     SqlTextStates.LC);         // --
            transitions.Add(SqlTextStates.HY, SqlTextChars.Null,            SqlTextStates.RC);
            // slash hold
            transitions.Add(SqlTextStates.WU, SqlTextChars.Slash,           SqlTextStates.SL);         // /
            transitions.Add(SqlTextStates.SL, SqlTextChars.Asterisk,        SqlTextStates.BC);         // /*
            transitions.Add(SqlTextStates.SL, SqlTextChars.Null,            SqlTextStates.RC);
            // capital N hold
            transitions.Add(SqlTextStates.WU, @"N",                         SqlTextStates.CN);         // N
            transitions.Add(SqlTextStates.CN, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // N'
            transitions.Add(SqlTextStates.CN, SqlTextChars.Null,            SqlTextStates.RC);
            // left-bracket path
            transitions.Add(SqlTextStates.WU, SqlTextChars.LeftBracket,     SqlTextStates.LB);         // [
            transitions.Add(SqlTextStates.LB, SqlTextChars.RightBracket,    SqlTextStates.RB);         // []
            transitions.Add(SqlTextStates.RB, SqlTextChars.RightBracket,    SqlTextStates.LB);         // []]
            transitions.Add(SqlTextStates.RB, SqlTextChars.Null,            SqlTextStates.RC);         // [] >> !"]"
            // quote path
            transitions.Add(SqlTextStates.WU, SqlTextChars.Quote,           SqlTextStates.QT);         // "
            transitions.Add(SqlTextStates.QT, SqlTextChars.Quote,           SqlTextStates.QE);         // ""
            transitions.Add(SqlTextStates.QE, SqlTextChars.Quote,           SqlTextStates.QT);         // """
            transitions.Add(SqlTextStates.QE, SqlTextChars.Null,            SqlTextStates.RC);         // "" >> !'"'
            // single-quote path
            transitions.Add(SqlTextStates.WU, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // '
            transitions.Add(SqlTextStates.AP, SqlTextChars.Apostrophe,      SqlTextStates.AE);         // '' or N''
            transitions.Add(SqlTextStates.AE, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // ''' or N'''
            transitions.Add(SqlTextStates.AE, SqlTextChars.Null,            SqlTextStates.RC);         // '' or N'' >> !"'"
            // block-comment path (entry is triggered with 49, see notes above)
            transitions.Add(SqlTextStates.BX, SqlTextChars.Asterisk,        SqlTextStates.BA);         // /* >> *
            transitions.Add(SqlTextStates.BX, SqlTextChars.Slash,           SqlTextStates.BS);         // /* >> /
            transitions.Add(SqlTextStates.BA, SqlTextChars.Asterisk,        SqlTextStates.BA);         // /* >> **
            transitions.Add(SqlTextStates.BA, SqlTextChars.Slash,           SqlTextStates.BC);         // /* >> */             // decrement block-comment counter and check
            transitions.Add(SqlTextStates.BA, SqlTextChars.Null,            SqlTextStates.BX);         // /* >> * >> !"/"
            transitions.Add(SqlTextStates.BS, SqlTextChars.Slash,           SqlTextStates.BS);         // /* >> //
            transitions.Add(SqlTextStates.BS, SqlTextChars.Asterisk,        SqlTextStates.BC);         // /* >> /*             // increment block-comment counter
            transitions.Add(SqlTextStates.BS, SqlTextChars.Null,            SqlTextStates.BX);         // /* >> / >> !"*"
            // line-comment path
            transitions.Add(SqlTextStates.LC, SqlTextChars.CarriageReturn,  SqlTextStates.LI);
            transitions.Add(SqlTextStates.LC, SqlTextChars.LineFeed,        SqlTextStates.LI);
            // line yield transition
            transitions.Add(SqlTextStates.LI, SqlTextChars.CarriageReturn,  SqlTextStates.LI);         // cluster all carriage-return and line-feed combinations together
            transitions.Add(SqlTextStates.LI, SqlTextChars.LineFeed,        SqlTextStates.LI);         // for the purpose of yielding lines.
            transitions.Add(SqlTextStates.LI, SqlTextChars.Null,            SqlTextStates.YR);

            return transitions;
        }

    }

}