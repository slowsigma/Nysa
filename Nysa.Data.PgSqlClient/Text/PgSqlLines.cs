using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.PgSql
{

    internal static class PgSqlLines
    {

        public static StateTransitions<PgSqlState, Char> Transitions()
        {
            var transitions = new StateTransitions<PgSqlState, Char>();

            // Line transitions are only those transitions necessary for determining lines in a PL/PGSQL batch.

            // Notes On Line Transitions:
            //    1. State   0 is the basic state for white space and anything else not recognized.
            //    2. State  49 is a trapping state used to count comment blocks.  The state indicates either:
            //                 a. "/*" the block-comment counter is incremented and the state is set to 40
            //                 b. "*/" the block-comment counter is decremented and...
            //                    i. if counter is zero, the lexing logic yields and the state is set to 0, or
            //                    ii. the state is set to 40.
            //    3. State 888 tells the lexing logic to reset the current state to 0 and recheck the current character.
            //    4. State 999 tells the lexing logic to yield without the current character, reset current state to 0, and recheck the current character.
            //    5. The PgSqlTextChars.Null negator character is used for both a miss condition and the input termination condition.

            // white-space
            transitions.Add(PgSqlState.WU, PgSqlChars.Space,           PgSqlState.WU);
            transitions.Add(PgSqlState.WU, PgSqlChars.Tab,             PgSqlState.WU);
            transitions.Add(PgSqlState.WU, PgSqlChars.Null,            PgSqlState.WU);         // if the character is not "recognized" stay in WU
            // carriage-return and line-feed hold
            transitions.Add(PgSqlState.WU, PgSqlChars.CarriageReturn,  PgSqlState.LI);
            transitions.Add(PgSqlState.WU, PgSqlChars.LineFeed,        PgSqlState.LI);
            // hyphen hold
            transitions.Add(PgSqlState.WU, PgSqlChars.HyphenMinus,     PgSqlState.HY);         // -
            transitions.Add(PgSqlState.HY, PgSqlChars.HyphenMinus,     PgSqlState.LC);         // --
            transitions.Add(PgSqlState.HY, PgSqlChars.Null,            PgSqlState.RC);
            // slash hold
            transitions.Add(PgSqlState.WU, PgSqlChars.Slash,           PgSqlState.SL);         // /
            transitions.Add(PgSqlState.SL, PgSqlChars.Asterisk,        PgSqlState.BC);         // /*
            transitions.Add(PgSqlState.SL, PgSqlChars.Null,            PgSqlState.RC);
            // E(e) hold
            transitions.Add(PgSqlState.WU, 'E',                        PgSqlState.CE);         // E
            transitions.Add(PgSqlState.WU, 'e',                        PgSqlState.CE);         // e
            transitions.Add(PgSqlState.CE, PgSqlChars.Apostrophe,      PgSqlState.CA);         // E'
            transitions.Add(PgSqlState.CE, PgSqlChars.Null,            PgSqlState.RC);

            // single-quote path C-style
            transitions.Add(PgSqlState.CA, PgSqlChars.Apostrophe,      PgSqlState.CX);         // E'' or e''
            transitions.Add(PgSqlState.CX, PgSqlChars.Apostrophe,      PgSqlState.CA);         // E''' or e'''
            transitions.Add(PgSqlState.CA, '\\',                       PgSqlState.CQ);         // E'\ or e'\
            transitions.Add(PgSqlState.CQ, PgSqlChars.Null,            PgSqlState.CA);         // E'\ or e'\ >> ignore
            transitions.Add(PgSqlState.CX, PgSqlChars.Null,            PgSqlState.RC);         // E'' or e'' >> !"'"

            // quote path
            transitions.Add(PgSqlState.WU, PgSqlChars.Quote,           PgSqlState.QT);         // "
            transitions.Add(PgSqlState.QT, PgSqlChars.Quote,           PgSqlState.QE);         // ""
            transitions.Add(PgSqlState.QE, PgSqlChars.Quote,           PgSqlState.QT);         // """
            transitions.Add(PgSqlState.QE, PgSqlChars.Null,            PgSqlState.RC);         // "" >> !'"'
            // single-quote path tranditional
            transitions.Add(PgSqlState.WU, PgSqlChars.Apostrophe,      PgSqlState.AP);         // '
            transitions.Add(PgSqlState.AP, PgSqlChars.Apostrophe,      PgSqlState.AE);         // '' or N''
            transitions.Add(PgSqlState.AE, PgSqlChars.Apostrophe,      PgSqlState.AP);         // ''' or N'''
            transitions.Add(PgSqlState.AE, PgSqlChars.Null,            PgSqlState.RC);         // '' or N'' >> !"'"
            // block-comment path (entry is triggered with 49, see notes above)
            transitions.Add(PgSqlState.BX, PgSqlChars.Asterisk,        PgSqlState.BA);         // /* >> *
            transitions.Add(PgSqlState.BX, PgSqlChars.Slash,           PgSqlState.BS);         // /* >> /
            transitions.Add(PgSqlState.BA, PgSqlChars.Asterisk,        PgSqlState.BA);         // /* >> **
            transitions.Add(PgSqlState.BA, PgSqlChars.Slash,           PgSqlState.BC);         // /* >> */             // decrement block-comment counter and check
            transitions.Add(PgSqlState.BA, PgSqlChars.Null,            PgSqlState.BX);         // /* >> * >> !"/"
            transitions.Add(PgSqlState.BS, PgSqlChars.Slash,           PgSqlState.BS);         // /* >> //
            transitions.Add(PgSqlState.BS, PgSqlChars.Asterisk,        PgSqlState.BC);         // /* >> /*             // increment block-comment counter
            transitions.Add(PgSqlState.BS, PgSqlChars.Null,            PgSqlState.BX);         // /* >> / >> !"*"
            // line-comment path
            transitions.Add(PgSqlState.LC, PgSqlChars.CarriageReturn,  PgSqlState.LI);
            transitions.Add(PgSqlState.LC, PgSqlChars.LineFeed,        PgSqlState.LI);
            // line yield transition
            transitions.Add(PgSqlState.LI, PgSqlChars.CarriageReturn,  PgSqlState.LI);         // cluster all carriage-return and line-feed combinations together
            transitions.Add(PgSqlState.LI, PgSqlChars.LineFeed,        PgSqlState.LI);         // for the purpose of yielding lines.
            transitions.Add(PgSqlState.LI, PgSqlChars.Null,            PgSqlState.YR);

            return transitions;
        }

    }

}