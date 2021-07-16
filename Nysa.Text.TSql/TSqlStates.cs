using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.TSql
{

    internal static class SqlTextStates
    {
        public const Int32 WS = 0;     // White Space
        public const Int32 YC = 1;     // Yield Current (next state WS)
        public const Int32 OS = 2;     // Operator Start
        public const Int32 HY = 3;     // Hyphen
        public const Int32 SL = 4;     // Slash
        public const Int32 EX = 5;     // Exclamation
        public const Int32 LT = 6;     // Less Than
        public const Int32 PE = 7;     // Period
        public const Int32 DS = 8;     // Dollar Sign
        public const Int32 CN = 9;     // Capital N
        public const Int32 LB = 10;    // Left Bracket
        public const Int32 RB = 11;    // Right Bracket Check
        public const Int32 QT = 20;    // Quote
        public const Int32 QE = 21;    // Quote Escape Check
        public const Int32 AP = 30;    // Apostrophe
        public const Int32 AE = 31;    // Apostrophe Escape Check
        public const Int32 BX = 40;    // Block State (checking characters)
        public const Int32 BA = 41;    // Block Asterisk
        public const Int32 BS = 42;    // Block Slash
        public const Int32 BC = 49;    // Block Comment (increment/decrement count and check, goto WS or BX)
        public const Int32 LC = 50;    // Line Comment
        public const Int32 WD = 60;    // Word
        public const Int32 ZR = 70;    // Zero
        public const Int32 HX = 71;    // Hex
        public const Int32 NU = 80;    // Numeric (non-money)
        public const Int32 NA = 81;    // Numeric (state A)
        public const Int32 NB = 82;    // Numeric (state B)
        public const Int32 NC = 83;    // Numeric (state C)
        public const Int32 MO = 90;    // Money
        public const Int32 MA = 91;    // Money (state A)
        public const Int32 MB = 92;    // Money (state B)
        public const Int32 YR = 999;   // Yield Reset Recheck


        public const Int32 WU = 0;     // White Space or Unrecognized
        public const Int32 LI = 700;   // Line Character (CR or LF);
        public const Int32 RC = 888;   // Reset Recheck

    }

}