using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Text.PgSql
{

    internal struct PgSqlState : IEquatable<PgSqlState>
    {
        private static Int32 _Next = 0;

        public static readonly PgSqlState HY = new PgSqlState(_Next++);     // Hyphen
        public static readonly PgSqlState SL = new PgSqlState(_Next++);     // Slash
        public static readonly PgSqlState PE = new PgSqlState(_Next++);     // Period
        public static readonly PgSqlState CE = new PgSqlState(_Next++);     // Capital E
        public static readonly PgSqlState CA = new PgSqlState(_Next++);     // Apostrophe (C-Style)
        public static readonly PgSqlState CX = new PgSqlState(_Next++);     // Apostrophe Escape (C-Style)
        public static readonly PgSqlState CQ = new PgSqlState(_Next++);     // Backslash Escape (C-Style)
        public static readonly PgSqlState EC = new PgSqlState(_Next++);     // Escape
        public static readonly PgSqlState LB = new PgSqlState(_Next++);     // Left Bracket
        public static readonly PgSqlState RB = new PgSqlState(_Next++);     // Right Bracket Check
        public static readonly PgSqlState QT = new PgSqlState(_Next++);     // Quote
        public static readonly PgSqlState QE = new PgSqlState(_Next++);     // Quote Escape Check
        public static readonly PgSqlState AP = new PgSqlState(_Next++);     // Apostrophe
        public static readonly PgSqlState AE = new PgSqlState(_Next++);     // Apostrophe Escape Check
        public static readonly PgSqlState BX = new PgSqlState(_Next++);     // Block State (checking characters)
        public static readonly PgSqlState BA = new PgSqlState(_Next++);     // Block Asterisk
        public static readonly PgSqlState BS = new PgSqlState(_Next++);     // Block Slash
        public static readonly PgSqlState BC = new PgSqlState(_Next++);     // Block Comment (increment/decrement count and check, goto WS or BX)
        public static readonly PgSqlState LC = new PgSqlState(_Next++);     // Line Comment
        public static readonly PgSqlState YR = new PgSqlState(_Next++);     // Yield Reset Recheck

        public static readonly PgSqlState WU = new PgSqlState(_Next++);     // White Space or Unrecognized
        public static readonly PgSqlState LI = new PgSqlState(_Next++);     // Line Character (CR or LF);
        public static readonly PgSqlState RC = new PgSqlState(_Next++);     // Reset (to WU) & Recheck


        // instance members
        public Int32 Value { get; init; }
        public PgSqlState(Int32 value) { this.Value = value; }
        public Boolean Equals(PgSqlState other) => this.Value.Equals(other.Value);
    }

}