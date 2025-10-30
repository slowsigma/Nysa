using System;

using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript;

public record ParseTree(
    Node Root,
    Token[] Trivia,
    VbScriptLine[] Lines
);
