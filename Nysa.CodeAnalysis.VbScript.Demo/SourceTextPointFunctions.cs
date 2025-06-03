using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public static class SourceTextPointFunctions
{

    private static Boolean IsComment(Span<Char> chars)
    {
        foreach (var c in chars)
        {
            if (!Char.IsWhiteSpace(c) && c == '\'')
                return true;
            else if (!Char.IsWhiteSpace(c))
                return false;
        }

        return false;
    }

    private static IEnumerable<(Int32 Position, Int32 Length)> CommentLines(Char[] source)
    {
        var newLine = new Char[] { '\r', '\n' };
        var current = 0;
        var next    = 0;

        while (current < source.Length)
        {
            next = current;

            while (    (next < source.Length)
                    && (source[next] != '\r')
                    && (source[next] != '\n'))
                next++;

            if (source.Length <= next)
                yield break;

            if (current < next && IsComment(new Span<Char>((Char[])source, current, (next - current))))
                yield return (current, (next - current));

            current = next;

            while (    (current < source.Length)
                    && (source[current] == '\r' || source[current] == '\n'))
                current++;
        }
    }


    // public static IReadOnlyList<SourceTextPoint> ComputeSourceTextPoints(FlowDocument flowDocument, String content, IEnumerable<Token> orderedTokens, ColorKey colorKey)
    // {
    //     var points = new List<SourceTextPoint>();

    //     var black = new SolidColorBrush(Colors.Black);
    //     var green = new SolidColorBrush(Colors.Green);
    //     var start = flowDocument.ContentStart;

    //     var initial = new List<(Int32 Position, Int32 Length, Token? Token)>();

    //     foreach (var token in orderedTokens)
    //         initial.Add((Position: token.Span.Position, Length: token.Span.Length, Token: token));

    //     foreach (var comment in CommentLines(content.ToCharArray()))
    //         initial.Add((Position: comment.Position, Length: comment.Length, Token: null));

    //     while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
    //         start = start.GetNextContextPosition(LogicalDirection.Forward);

    //     foreach (var item in initial.OrderByDescending(p => p.Position))
    //     {
    //         var textColor = item.Token != null
    //                         ? colorKey.ColorFor(item.Token.Value.Id)
    //                         : green;

    //         var startPos = start.GetPositionAtOffset(item.Position, LogicalDirection.Forward);
    //         var endPos   = startPos.GetPositionAtOffset(item.Length, LogicalDirection.Forward);

    //         var range = new TextRange(startPos, endPos);

    //         points.Add(new SourceTextPoint(item.Position, item.Length, item.Token, range, textColor));
    //     }

    //     foreach (var item in points)
    //         item.Range.ApplyPropertyValue(TextElement.ForegroundProperty, item.Color);

    //     return points;
    // }
    
}