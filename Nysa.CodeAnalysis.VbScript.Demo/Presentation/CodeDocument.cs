using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Documents;
using System.Windows.Media;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.Documents;

public class CodeDocument
{
    public static CodeDocument Create(String code, Token[] tokens, ColorKey tokenColors, Double fontSize, IEnumerable<(Int32 Position, Int32 Length)> comments, Int32 commentColor)
    {
        var doc = new FlowDocument();
        var tps = new List<CodeTextPoint>();
        var prg = new Paragraph(new Run(code));

        prg.FontFamily = new FontFamily("Courier New");
        prg.FontSize = fontSize;

        doc.PageWidth = 2000;
        doc.Blocks.Add(prg);

        var initial = new List<(Int32 Position, Int32 Length, Token? Token)>();

        foreach (var token in tokens)
            initial.Add((Position: token.Span.Position, Length: token.Span.Length, Token: token));

        foreach (var comment in comments)
            initial.Add((Position: comment.Position, Length: comment.Length, Token: null));

        var start = doc.ContentStart;

        while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            start = start.GetNextContextPosition(LogicalDirection.Forward);

        foreach (var item in initial.OrderByDescending(p => p.Position))
        {
            var textColor = item.Token != null
                            ? tokenColors.ColorFor(item.Token.Value.Id)
                            : commentColor;

            var startPos = start.GetPositionAtOffset(item.Position, LogicalDirection.Forward);
            var endPos = startPos.GetPositionAtOffset(item.Length, LogicalDirection.Forward);

            var range = new TextRange(startPos, endPos);

            tps.Add(new CodeTextPoint(item.Position, item.Length, item.Token, range, textColor));
        }

        return new CodeDocument(doc, tps, code);
    }

    // instance level
    public FlowDocument Document { get; private set; }
    public IReadOnlyList<CodeTextPoint> TextPoints { get; private set; }
    public String Code { get; private set; }

    private CodeDocument(FlowDocument document, IReadOnlyList<CodeTextPoint> textPoints, String code)
    {
        this.Document = document;
        this.TextPoints = textPoints;
        this.Code = code;
    }

    /// <summary>
    /// Applies colors to the computed text ranges based on the given color index. 
    /// This can only be called on the foreground thread.
    /// </summary>
    public void ApplyColors(IReadOnlyList<SolidColorBrush> index)
    {
        foreach (var tp in this.TextPoints)
            tp.Range.ApplyPropertyValue(TextElement.ForegroundProperty, index[tp.ColorNumber]);
    }
}
