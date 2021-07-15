using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.Sql
{

    internal static class SqlTextTokens
    {

        public static StateTransitions<Int32, Char> Transitions()
        {
            var transitions = new StateTransitions<Int32, Char>();

            //    Pattern Checklist
            //    Name                             Rule
            //    -------------------------------  ------------------------------------------------------------------------------------
            //    Singular-Symbol                  >>   ( "(" | ")" | "," | ":" | ";" | "=" | "~" )
            //    Line-Comment                     >>   "--" >> {any-character}* >> ( {cr} | {lf} )
            //    Block-Comment                    >>   "/*" >> ( {any-character} | block-comment )* >> "*/"
            //    Identifier                       >>   "["  >> ( {any-character} | "]]" )* >> "]" >> !"]"
            //    Word                             >>   ( "#" | "@" | "_" | [a-z] | [A-Z] ) >> ( "#" | "@" | "_" | [a-z] | [A-Z] ) [0-9] )*
            //    Literal-String                   >>   "N"? >> "'" >> ( {any-character} | "''" )* >> "'" >> !"'"
            //    Literal-Money                    >>   "$" >> " "* >> [0-9]* >> "."? >> [0-9]*
            //    Literal-Binary                   >>   "0" >> [Xx] >> ( [0-9] | [a-f] | [A-F] )*
            //    Literal-Decimal                  >>   [0-9]* >> "."? >> [0-9]*
            //    Literal-Integer                  >>   [0-9]*
            //    Literal-Float                    >>   [0-9]* >> "."? >> [0-9]* >> [Ee]? >> "-"? >> [0-9]*

            // Notes On Base Transitions:
            //    1. State   0 is the "white space" consuming state.
            //    2. State   1 is the only state that immediately yields the current character and automatically returns the state back to zero.
            //    3. State  49 is a trapping state used to count comment blocks.  The state indicates either:
            //                 a. "/*" the block-comment counter is incremented and the state is set to 40
            //                 b. "*/" the block-comment counter is decremented and...
            //                    i. if counter is zero, the lexing logic yields and the state is set to 0, or
            //                    ii. the state is set to 40.
            //    4. State 999 tells the lexing logic to yield without the current character, reset current state to 0, and recheck the current character.
            //    5. The SqlTextChars.Null negator character is used for both a miss condition and the input termination condition.

            // white-space
            transitions.Add(SqlTextStates.WS, SqlTextChars.WhiteSpace,      SqlTextStates.WS);         // from "white space" to more "white space" stay in zero
            transitions.Add(SqlTextStates.WS, SqlTextChars.Termination,     SqlTextStates.WS);
            transitions.Add(SqlTextStates.WS, SqlTextChars.Symbol,          SqlTextStates.YC);         // ( ) , : ; = ~
            // symbol hold for equals
            transitions.Add(SqlTextStates.WS, SqlTextChars.OperatorStarts,  SqlTextStates.OS);         // %  &  *  +  ^  |
            transitions.Add(SqlTextStates.WS, SqlTextChars.GreaterThan,     SqlTextStates.OS);         //                   >
            transitions.Add(SqlTextStates.OS, SqlTextChars.EqualsSign,      SqlTextStates.YC);         // %= &= *= += ^= |= >=
            transitions.Add(SqlTextStates.OS, SqlTextChars.Null,            SqlTextStates.YR);
            // hyphen hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.HyphenMinus,     SqlTextStates.HY);         // -
            transitions.Add(SqlTextStates.HY, SqlTextChars.EqualsSign,      SqlTextStates.YC);         // -=
            transitions.Add(SqlTextStates.HY, SqlTextChars.HyphenMinus,     SqlTextStates.LC);         // --
            transitions.Add(SqlTextStates.HY, SqlTextChars.Null,            SqlTextStates.YR);
            // slash hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.Slash,           SqlTextStates.SL);         // /
            transitions.Add(SqlTextStates.SL, SqlTextChars.EqualsSign,      SqlTextStates.YC);         // /=
            transitions.Add(SqlTextStates.SL, SqlTextChars.Asterisk,        SqlTextStates.BC);         // /*
            transitions.Add(SqlTextStates.SL, SqlTextChars.Null,            SqlTextStates.YR);
            // exclamation hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.Exclamation,     SqlTextStates.EX);         // !
            transitions.Add(SqlTextStates.EX, @"<=>",                       SqlTextStates.YC);         // !< != !>
            transitions.Add(SqlTextStates.EX, SqlTextChars.Null,            SqlTextStates.YR);
            // less-than hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.LessThan,        SqlTextStates.LT);         // <
            transitions.Add(SqlTextStates.LT, @"=>",                        SqlTextStates.YC);         // <= <>
            transitions.Add(SqlTextStates.LT, SqlTextChars.Null,            SqlTextStates.YR);
            // period hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.Period,          SqlTextStates.PE);         // .
            transitions.Add(SqlTextStates.PE, SqlTextChars.DecimalDigit,    SqlTextStates.NA);         // . >> [0-9]
            transitions.Add(SqlTextStates.PE, SqlTextChars.Null,            SqlTextStates.YR);         // 
            // dollar-sign hold
            transitions.Add(SqlTextStates.WS, SqlTextChars.DollarSign,      SqlTextStates.DS);         // $
            transitions.Add(SqlTextStates.DS, SqlTextChars.WordStartAll,    SqlTextStates.WD);         // pseudo-column
            transitions.Add(SqlTextStates.DS, SqlTextChars.Space,           SqlTextStates.MO);         // $ >> " "
            transitions.Add(SqlTextStates.DS, SqlTextChars.DecimalDigit,    SqlTextStates.MA);         // $ >> [0-9]
            transitions.Add(SqlTextStates.DS, SqlTextChars.Period,          SqlTextStates.MB);         // $.
            transitions.Add(SqlTextStates.DS, SqlTextChars.Null,            SqlTextStates.YR);
            // capital N hold
            transitions.Add(SqlTextStates.WS, @"N",                         SqlTextStates.CN);         // N
            transitions.Add(SqlTextStates.CN, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // N'
            transitions.Add(SqlTextStates.CN, SqlTextChars.WordContinue,    SqlTextStates.WD);         // word
            transitions.Add(SqlTextStates.CN, SqlTextChars.Null,            SqlTextStates.YR);
            // left-bracket path
            transitions.Add(SqlTextStates.WS, SqlTextChars.LeftBracket,     SqlTextStates.LB);         // [
            transitions.Add(SqlTextStates.LB, SqlTextChars.RightBracket,    SqlTextStates.RB);         // []
            transitions.Add(SqlTextStates.RB, SqlTextChars.RightBracket,    SqlTextStates.LB);         // []]
            transitions.Add(SqlTextStates.RB, SqlTextChars.Null,            SqlTextStates.YR);         // [] >> !"]"
            // quote path
            transitions.Add(SqlTextStates.WS, SqlTextChars.Quote,           SqlTextStates.QT);         // "
            transitions.Add(SqlTextStates.QT, SqlTextChars.Quote,           SqlTextStates.QE);         // ""
            transitions.Add(SqlTextStates.QE, SqlTextChars.Quote,           SqlTextStates.QT);         // """
            transitions.Add(SqlTextStates.QE, SqlTextChars.Null,            SqlTextStates.YR);         // "" >> !'"'
            // single-quote path
            transitions.Add(SqlTextStates.WS, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // '
            transitions.Add(SqlTextStates.AP, SqlTextChars.Apostrophe,      SqlTextStates.AE);         // '' or N''
            transitions.Add(SqlTextStates.AE, SqlTextChars.Apostrophe,      SqlTextStates.AP);         // ''' or N'''
            transitions.Add(SqlTextStates.AE, SqlTextChars.Null,            SqlTextStates.YR);         // '' or N'' >> !"'"
            // block-comment path (entry is triggered with 49 (BC), see notes above)
            transitions.Add(SqlTextStates.BX, SqlTextChars.Asterisk,        SqlTextStates.BA);         // /* >> *
            transitions.Add(SqlTextStates.BX, SqlTextChars.Slash,           SqlTextStates.BS);         // /* >> /
            transitions.Add(SqlTextStates.BA, SqlTextChars.Asterisk,        SqlTextStates.BA);         // /* >> **
            transitions.Add(SqlTextStates.BA, SqlTextChars.Slash,           SqlTextStates.BC);         // /* >> */             // decrement block-comment counter and check
            transitions.Add(SqlTextStates.BA, SqlTextChars.Null,            SqlTextStates.BX);         // /* >> * >> !"/"
            transitions.Add(SqlTextStates.BS, SqlTextChars.Slash,           SqlTextStates.BS);         // /* >> //
            transitions.Add(SqlTextStates.BS, SqlTextChars.Asterisk,        SqlTextStates.BC);         // /* >> /*             // increment block-comment counter
            transitions.Add(SqlTextStates.BS, SqlTextChars.Null,            SqlTextStates.BX);         // /* >> / >> !"*"
            // line-comment path
            transitions.Add(SqlTextStates.LC, SqlTextChars.CarriageReturn,  SqlTextStates.YR);
            transitions.Add(SqlTextStates.LC, SqlTextChars.LineFeed,        SqlTextStates.YR);
            // plain word path (note: SqlTextChars.WordStartBase does not include the capital letter "N")
            transitions.Add(SqlTextStates.WS, SqlTextChars.WordStartNoN,    SqlTextStates.WD);
            transitions.Add(SqlTextStates.WD, SqlTextChars.WordContinue,    SqlTextStates.WD);
            transitions.Add(SqlTextStates.WD, SqlTextChars.Null,            SqlTextStates.YR);
            // zero hold and varbinary path
            transitions.Add(SqlTextStates.WS, @"0",                         SqlTextStates.ZR);         // "0"
            transitions.Add(SqlTextStates.ZR, @"Xx",                        SqlTextStates.HX);         // "0" >> [Xx]
            transitions.Add(SqlTextStates.ZR, SqlTextChars.DecimalDigit,    SqlTextStates.NU);         // "0" >> [0-9]
            transitions.Add(SqlTextStates.ZR, SqlTextChars.Period,          SqlTextStates.NA);         // "0" >> "."
            transitions.Add(SqlTextStates.ZR, @"Ee",                        SqlTextStates.NB);         // "0" >> [Ee]
            transitions.Add(SqlTextStates.ZR, SqlTextChars.Null,            SqlTextStates.YR);
            transitions.Add(SqlTextStates.HX, SqlTextChars.Hexadecimal,     SqlTextStates.HX);         // "0" >> [Xx] >> ([0-9] or [A-F] or [a-f])+
            transitions.Add(SqlTextStates.HX, SqlTextChars.Null,            SqlTextStates.YR);
            // non-money numeric literal path
            transitions.Add(SqlTextStates.WS, @"123456789",                 SqlTextStates.NU);         // [1-9]
            transitions.Add(SqlTextStates.NU, SqlTextChars.DecimalDigit,    SqlTextStates.NU);         // [0-9]+
            transitions.Add(SqlTextStates.NU, SqlTextChars.Period,          SqlTextStates.NA);         // [0-9]+ >> "."
            transitions.Add(SqlTextStates.NU, @"Ee",                        SqlTextStates.NB);         // [0-9]+ >> [Ee]
            transitions.Add(SqlTextStates.NU, SqlTextChars.Null,            SqlTextStates.YR);         // [0-9]+ >> !"." and ![Ee]
            transitions.Add(SqlTextStates.NA, SqlTextChars.DecimalDigit,    SqlTextStates.NA);         // [0-9]* >> "." >> [0-9]+
            transitions.Add(SqlTextStates.NA, @"Ee",                        SqlTextStates.NB);         // [0-9]* >> "." >> [0-9]+ >> [Ee]
            transitions.Add(SqlTextStates.NA, SqlTextChars.Null,            SqlTextStates.YR);         // [0-9]* >> "." >> [0-9]+ >> ![0-9] and ![Ee]
            transitions.Add(SqlTextStates.NB, SqlTextChars.DecimalDigit,    SqlTextStates.NC);         // [0-9]* >> "."? >> [0-9]+ >> [Ee] >> [0-9]
            transitions.Add(SqlTextStates.NB, SqlTextChars.HyphenMinus,     SqlTextStates.NC);         // [0-9]* >> "."? >> [0-9]+ >> [Ee] >> "-"
            transitions.Add(SqlTextStates.NB, SqlTextChars.Null,            SqlTextStates.YR);         // [0-9]* >> "."? >> [0-9]+ >> [Ee] >> ![0-9] and !"-"
            transitions.Add(SqlTextStates.NC, SqlTextChars.DecimalDigit,    SqlTextStates.NC);         // [0-9]* >> "."? >> [0-9]+ >> [Ee] >> [0-9] or "-" >> [0-9]+
            transitions.Add(SqlTextStates.NC, SqlTextChars.Null,            SqlTextStates.YR);
            // literal money path (started with dollar-sign hold)
            transitions.Add(SqlTextStates.MO, SqlTextChars.Space,           SqlTextStates.MO);         // "$" >> " "+
            transitions.Add(SqlTextStates.MO, SqlTextChars.DecimalDigit,    SqlTextStates.MA);         // "$" >> " "+ >> [0-9]
            transitions.Add(SqlTextStates.MO, SqlTextChars.Period,          SqlTextStates.MB);         // "$" >> " "+ >> "."
            transitions.Add(SqlTextStates.MO, SqlTextChars.Null,            SqlTextStates.YR);         // "$" >> " "+ >> ![0-9] and !"."
            transitions.Add(SqlTextStates.MA, SqlTextChars.DecimalDigit,    SqlTextStates.MA);         // "$" >> " "* >> [0-9]+
            transitions.Add(SqlTextStates.MA, SqlTextChars.Period,          SqlTextStates.MB);         // "$" >> " "* >> [0-9]+ >> "."
            transitions.Add(SqlTextStates.MA, SqlTextChars.Null,            SqlTextStates.YR);         // "$" >> " "* >> [0-9]+ >> ![0-9] and !"."
            transitions.Add(SqlTextStates.MB, SqlTextChars.DecimalDigit,    SqlTextStates.MB);         // "$" >> " "* >> [0-9]* >> "."? >> [0-9]
            transitions.Add(SqlTextStates.MB, SqlTextChars.Null,            SqlTextStates.YR);         // "$" >> " "* >> [0-9]* >> "."? >> [0-9]* >> ![0-9]

            return transitions;
        }

    }

}