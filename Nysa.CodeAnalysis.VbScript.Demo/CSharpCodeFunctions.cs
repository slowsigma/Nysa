using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Nysa.CodeAnalysis.Documents;
using Nysa.WpfLx;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public static class CSharpCodeFunctions
{
    private record struct CSharpTextPoint(
       Int32 AbsolutePosition,
       Int32 AbsoluteLength,
       TextRange Range,
       SolidColorBrush Color
    );


    public static FlowDocument Colorize(String code)
    {
        var points = new List<CSharpTextPoint>();
        var colorKey = new CSharpColorKey(null);
        var doc = new FlowDocument();
        var tokens = SyntaxFactory.ParseTokens(code, 0, 0, CSharpParseOptions.Default);
        var trivia = Brushes.Green;
        var current = 0;

        var prg = new Paragraph(new Run(code));

        prg.FontFamily = new FontFamily("Courier New");
        prg.FontSize = 14.0;

        doc.PageWidth = 2000;
        doc.Blocks.Add(prg);

        var start = doc.ContentStart;

        while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            start = start.GetNextContextPosition(LogicalDirection.Forward);

        foreach (var token in tokens)
        {
            var kind = token.Kind();
            var color = colorKey.GetColor(kind);

            if (token.HasLeadingTrivia)
            {
                foreach (var item in token.LeadingTrivia.Where(t => colorKey.IsComment(t.Kind())))
                {
                    var trKind = item.Kind();
                    var trPos = start.GetPositionAtOffset(item.Span.Start, LogicalDirection.Forward);
                    var trLen = trPos.GetPositionAtOffset(item.Span.Length, LogicalDirection.Forward);

                    var trRange = new TextRange(trPos, trLen);

                    points.Add(new CSharpTextPoint(item.Span.Start, item.Span.Length, trRange, trivia));
                }
            }

            var startPos = start.GetPositionAtOffset(token.Span.Start, LogicalDirection.Forward);
            var endPos = startPos.GetPositionAtOffset(token.Span.Length, LogicalDirection.Forward);

            var range = new TextRange(startPos, endPos);

            points.Add(new CSharpTextPoint(token.Span.Start, token.Span.Length, range, color));

            if (token.HasTrailingTrivia)
            {
                foreach (var item in token.TrailingTrivia.Where(t => colorKey.IsComment(t.Kind())))
                {
                    var trPos = start.GetPositionAtOffset(item.Span.Start, LogicalDirection.Forward);
                    var trLen = trPos.GetPositionAtOffset(item.Span.Length, LogicalDirection.Forward);

                    var trRange = new TextRange(trPos, trLen);

                    points.Add(new CSharpTextPoint(item.Span.Start, item.Span.Length, trRange, trivia));
                }
            }
        }

        foreach (var point in points)
            point.Range.ApplyPropertyValue(TextElement.ForegroundProperty, point.Color);

        return doc;
    }

}