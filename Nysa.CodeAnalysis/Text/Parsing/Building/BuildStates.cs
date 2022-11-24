using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

internal struct BuildStates : IEquatable<BuildStates>
{
    // static members
    public static readonly BuildStates ACROSS_CALL = new BuildStates(0);
    public static readonly BuildStates ACROSS_MATCH = new BuildStates(1);
    public static readonly BuildStates ACROSS_MATCH_DOWN_CHECK = new BuildStates(2);
    public static readonly BuildStates ACROSS_MATCH_ACROSS_CHECK = new BuildStates(3);
    public static readonly BuildStates DOWN_CALL = new BuildStates(4);
    public static readonly BuildStates DOWN_MATCH = new BuildStates(5);
    public static readonly BuildStates DOWN_MATCH_DOWN_CHECK = new BuildStates(6);
    public static readonly BuildStates DOWN_MATCH_ACROSS_CHECK = new BuildStates(7);
    public static readonly BuildStates RETURN = new BuildStates(8);
    public static readonly BuildStates FINAL = new BuildStates(9);

    public static implicit operator Int32(BuildStates buildState) => buildState._Value;
    public static Boolean operator ==(BuildStates lhs, BuildStates rhs) => lhs.Equals(rhs);
    public static Boolean operator !=(BuildStates lhs, BuildStates rhs) => !lhs.Equals(rhs);

    // instance members
    private Int32 _Value;
    private BuildStates(Int32 value) { this._Value = value; }

    public Boolean Equals(BuildStates other)
        => this._Value.Equals(other._Value);
    public override Int32 GetHashCode()
        => this._Value.GetHashCode();
    public override Boolean Equals(Object? other)
        => other is BuildStates bs ? this.Equals(bs) : false;
}
