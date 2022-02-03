using System;
using System.Collections.Generic;

using SyntaxNode = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public enum OperationTypes : Int32
    {
        Implication,    // Imp
        Equivalence,    // Eqv
        ExclusiveOr,    // Xor
        Or,             // Or
        And,            // And
        Not,            // Not
        Is,             // Is
        IsNot,          // Is Not
        GreaterOrEqual, // >= =>
        LesserOrEqual,  // <= =<
        Greater,        // >
        Lesser,         // <
        NotEqual,       // <>
        Equal,          // =
        Concatenate,    // &
        Add,            // +    (unary is SignPositive)
        Subtract,       // -    (unary is SignNegative)
        Mod,            // Mod
        IntDivide,      // \
        Multiply,       // *
        Divide,         // /
        Exponentiate,   // ^
        SignPositive,
        SignNegative
    }

}
