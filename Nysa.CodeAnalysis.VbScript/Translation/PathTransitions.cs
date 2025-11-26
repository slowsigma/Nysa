using System;
using System.Collections.Generic;

namespace Nysa.CodeAnalysis.VbScript;

public sealed class PathTransitions
{
    // Even though some of these transitions might seem like errors, they are kept here
    // in case a part of a path expression is incorrectly identified.
    public static readonly PathTransitions DotTransitions = new PathTransitions(
        new[]
        {
            PathTranslateState.Variable,
            PathTranslateState.Constant,
            PathTranslateState.Property,
            PathTranslateState.Function,
            PathTranslateState.ArrayArguments,
            PathTranslateState.FunctionArguments
        },
        new[]
        {
            PathTranslateState.Variable,
            PathTranslateState.Constant,
            PathTranslateState.Property,
            PathTranslateState.Function,
        }
    );


    // instance members
    public IReadOnlySet<PathTranslateState> FromStates { get; private set; }
    public IReadOnlySet<PathTranslateState> ToStates { get; private set; }

    private PathTransitions(IEnumerable<PathTranslateState> from, IEnumerable<PathTranslateState> to)
    {
        this.FromStates = new HashSet<PathTranslateState>(from);
        this.ToStates = new HashSet<PathTranslateState>(to);
    }
}