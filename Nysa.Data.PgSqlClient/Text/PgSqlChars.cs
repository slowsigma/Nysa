using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.PgSql
{

    internal static class PgSqlChars
    {
        public static readonly Char Null              = '\u0000'; // ascii 0
        public static readonly Char Tab               = '\u0009'; // ascii 9
        public static readonly Char LineFeed          = '\u000A'; // ascii 10
        public static readonly Char CarriageReturn    = '\u000D'; // ascii 13
        public static readonly Char Space             = '\u0020'; // ascii 32
        public static readonly Char Exclamation       = '\u0021'; // !            requires one of _charsOperatorEnds
        public static readonly Char Quote             = '\u0022'; // "
        public static readonly Char NumberSign        = '\u0023'; // #
        public static readonly Char DollarSign        = '\u0024'; // $
        public static readonly Char Apostrophe        = '\u0027'; // '
        public static readonly Char Asterisk          = '\u002A'; // *
        public static readonly Char Plus              = '\u002B'; // +
        public static readonly Char Comma             = '\u002C'; // ,
        public static readonly Char HyphenMinus       = '\u002D'; // -
        public static readonly Char Period            = '\u002E'; // .
        public static readonly Char Slash             = '\u002F'; // /
        public static readonly Char Colon             = '\u003A'; // :
        public static readonly Char LessThan          = '\u003C'; // <
        public static readonly Char EqualsSign        = '\u003D'; // =
        public static readonly Char GreaterThan       = '\u003E'; // >
        public static readonly Char AtSign            = '\u0040'; // @
        public static readonly Char Backslash         = '\u005C'; // \            string breaking is not recognized by this class
        public static readonly Char Underscore        = '\u005F'; // _

        public static readonly Char Initiation        = '\u0080'; // meaningless value used for starting StateValue
        public static readonly Char Termination       = '\u0081'; // not used

        public static readonly String WhiteSpace     = String.Concat(Tab, LineFeed, CarriageReturn, Space);
        public static readonly String OperatorStarts = @"%&*+^|";                        // note: "-" and "/" have their own path
        public static readonly String Symbol         = @"(),:;=~";                       // note: "." has its own path
        public static readonly String DecimalDigit   = @"0123456789";
        public static readonly String UppercaseAtoZ  = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";    // note: "N" has its own path
        public static readonly String LowercaseAtoZ  = @"abcdefghijklmnopqrstuvwxyz";

        public static readonly String Hexadecimal    = String.Concat(DecimalDigit, @"ABCDEFabcdef");

        public static readonly String WordStartNoN   = String.Concat(NumberSign, AtSign, Underscore, @"ABCDEFGHIJKLMOPQRSTUVWXYZ", LowercaseAtoZ);
        public static readonly String WordStartAll   = String.Concat(NumberSign, AtSign, Underscore, UppercaseAtoZ, LowercaseAtoZ);
        public static readonly String WordContinue   = String.Concat(WordStartAll, DecimalDigit);

    }

}