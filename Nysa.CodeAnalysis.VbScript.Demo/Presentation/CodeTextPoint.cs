using System;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.Documents;

public record struct CodeTextPoint(
    Int32 AbsolutePosition,
    Int32 AbsoluteLength,
    Token? Token,
    TextRange Range,
    Int32 ColorNumber
);
