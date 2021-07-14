using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.Sql
{

    public static class SqlText
    {
        private static readonly Char _charNull              = '\u0000'; // ascii 0
        private static readonly Char _charTab               = '\u0009'; // ascii 9
        private static readonly Char _charLineFeed          = '\u000A'; // ascii 10
        private static readonly Char _charCarriageReturn    = '\u000D'; // ascii 13
        private static readonly Char _charSpace             = '\u0020'; // ascii 32
        private static readonly Char _charExclamation       = '\u0021'; // !            requires one of _charsOperatorEnds
        private static readonly Char _charQuote             = '\u0022'; // "
        private static readonly Char _charNumberSign        = '\u0023'; // #
        private static readonly Char _charDollarSign        = '\u0024'; // $
        private static readonly Char _charApostrophe        = '\u0027'; // '
        private static readonly Char _charAsterisk          = '\u002A'; // *
        private static readonly Char _charPlus              = '\u002B'; // +
        private static readonly Char _charComma             = '\u002C'; // ,
        private static readonly Char _charHyphenMinus       = '\u002D'; // -
        private static readonly Char _charPeriod            = '\u002E'; // .
        private static readonly Char _charSlash             = '\u002F'; // /
        private static readonly Char _charColon             = '\u003A'; // :
        private static readonly Char _charLessThan          = '\u003C'; // <
        private static readonly Char _charEqualsSign        = '\u003D'; // =
        private static readonly Char _charGreaterThan       = '\u003E'; // >
        private static readonly Char _charAtSign            = '\u0040'; // @
        private static readonly Char _charLeftBracket       = '\u005B'; // [
        private static readonly Char _charBackslash         = '\u005C'; // \            string breaking is not recognized by this class
        private static readonly Char _charRightBracket      = '\u005D'; // ]
        private static readonly Char _charUnderscore        = '\u005F'; // _

        private static readonly Char _charInitiation        = '\u0080';
        private static readonly Char _charTermination       = '\u0081';

        private static readonly String _charsWhiteSpace     = String.Concat(_charTab, _charLineFeed, _charCarriageReturn, _charSpace);
        private static readonly String _charsOperatorStarts = @"%&*+^|";                        // note: "-" and "/" have their own path
        private static readonly String _charsSymbol         = @"(),:;=~";                       // note: "." has its own path
        private static readonly String _charsDecimalDigit   = @"0123456789";
        private static readonly String _charsUppercaseAtoZ  = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";    // note: "N" has its own path
        private static readonly String _charsLowercaseAtoZ  = @"abcdefghijklmnopqrstuvwxyz";

        private static readonly String _charsHexadecimal    = String.Concat(_charsDecimalDigit, @"ABCDEFabcdef");

        private static readonly String _charsWordStartNoN   = String.Concat(_charNumberSign, _charAtSign, _charUnderscore, @"ABCDEFGHIJKLMOPQRSTUVWXYZ", _charsLowercaseAtoZ);
        private static readonly String _charsWordStartAll   = String.Concat(_charNumberSign, _charAtSign, _charUnderscore, _charsUppercaseAtoZ, _charsLowercaseAtoZ);
        private static readonly String _charsWordContinue   = String.Concat(_charsWordStartAll, _charsDecimalDigit);

        private static readonly StateTransitions<Int32, Char> _SQLBaseTransitions;
        private static readonly StateTransitions<Int32, Char> _SQLLineTransitions;

        static SqlText()
        {
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
            //    3. State  49 is a non-consuming state used to tell the lexing logic to check the current character.
            //                 a. For "*" the block-comment counter is incremented and the state is set to 40
            //                 b. For "/" the block-comment counter is decremented and...
            //                    i. if counter is zero, the lexing logic yields and the state is set to 0, or
            //                    ii. the state is set to 40.
            //    4. State 999 tells the lexing logic to yield without the current character, reset current state to 0, and recheck the current character.
            //    5. The _charNull negator character is used for both a miss condition and the input termination condition.

        }

    }

}