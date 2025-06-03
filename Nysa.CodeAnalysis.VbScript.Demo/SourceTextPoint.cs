using System;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public record struct SourceTextPoint(
    Int32 Position,
    Int32 Length,
    Token? Token,
    TextRange Range,
    SolidColorBrush Color
);
