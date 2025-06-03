using System;
using System.Windows.Media;
using Nysa.CodeAnalysis.Documents;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed class VbScriptColorKey : ColorKey
{
    public static SolidColorBrush[] Index = new SolidColorBrush[]
    {
        new SolidColorBrush(Colors.Black),
        new SolidColorBrush(Colors.Blue),
        new SolidColorBrush(Colors.Green),
        new SolidColorBrush(Colors.Red)
    };

    public static Int32 Black = 0;
    public static Int32 Blue = 1;
    public static Int32 Green = 2;
    public static Int32 Red = 3;

    public static Int32 DefaultColor = 0; // black

    public VbScriptColorKey()
        : base(VbScriptColorKey.Black)
    {
        var grammar    = VbScript.Language.Grammar;

        base.AddRange(grammar.LiteralSymbols().Where(s => s.All(c => Char.IsLetter(c))).Select(s => grammar.Id(s)), VbScriptColorKey.Blue);

        base.Add(grammar.Id("{StringLiteral}"), VbScriptColorKey.Red);
        base.Add(grammar.Id("{IntLiteral}"), VbScriptColorKey.Green);
        base.Add(grammar.Id("{FloatLiteral}"), VbScriptColorKey.Green);
        base.Add(grammar.Id("{DateLiteral}"), VbScriptColorKey.Green);
        base.Add(grammar.Id("{HexLiteral}"), VbScriptColorKey.Green);
        base.Add(grammar.Id("{OctLiteral}"), VbScriptColorKey.Green);
    }

}