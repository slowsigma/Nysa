using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;

namespace Nysa.Text;


public struct TextPosition : IEquatable<TextPosition>, IComparable<TextPosition>
{
    // instance members
    public static Boolean operator ==(TextPosition lhs, TextPosition rhs) => lhs.Equals(rhs);
    public static Boolean operator !=(TextPosition lhs, TextPosition rhs) => !lhs.Equals(rhs);
    public static Boolean operator >(TextPosition lhs, TextPosition rhs) => lhs.Source == rhs.Source && lhs.Value > rhs.Value;
    public static Boolean operator <(TextPosition lhs, TextPosition rhs) => lhs.Source == rhs.Source && lhs.Value < rhs.Value;
    public static Boolean operator >=(TextPosition lhs, TextPosition rhs) => lhs.Source == rhs.Source && lhs.Value >= rhs.Value;
    public static Boolean operator <=(TextPosition lhs, TextPosition rhs) => lhs.Source == rhs.Source && lhs.Value <= rhs.Value;
    public static TextPosition operator +(TextPosition lhs, Int32 rhs) => new TextPosition(lhs.Source, lhs.Value + rhs);
    public static TextPosition operator +(Int32 lhs, TextPosition rhs) => new TextPosition(rhs.Source, rhs.Value + lhs);

    // instance members
    public String Source { get; private set; }
    public Int32 Value { get; private set; }
    public Boolean IsSourceStart() => this.Value == 0;
    public Boolean IsSourceEnd() => !(this.Value < this.Source.Length);

    public TextPosition(String source, Int32 value)
    {
        this.Source = source ?? String.Empty;
        this.Value = value < 0 ? 0
                      : value < this.Source.Length ? value
                      : this.Source.Length;
    }

    public Boolean Equals(TextPosition other) => this.Source == other.Source && this.Value == other.Value;
    public override Boolean Equals(Object? obj) => obj is TextPosition pos ? this.Equals(pos) : false;
    public override Int32 GetHashCode() => this.Source.HashWithOther(this.Value);
    public Int32 CompareTo(TextPosition other) => this.Value.CompareTo(other.Value);
}
