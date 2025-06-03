using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace Nysa.CodeAnalysis.Documents;


public record struct TextPoint(
    Int32 AbsolutePosition,
    Int32 AbsoluteLength,
    TextRange Range,
    Int32 ColorNumber
);
