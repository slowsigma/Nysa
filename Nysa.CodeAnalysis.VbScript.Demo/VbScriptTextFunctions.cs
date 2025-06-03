using System;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public static class VbScriptTextFunctions
{

    public static Boolean IsComment(this ReadOnlySpan<Char> chars)
    {
        foreach (var c in chars)
        {
            if (!Char.IsWhiteSpace(c))  // keep going while we have white space
                return c == '\'';       // return true only if the first char is apostrophe
        }

        return false;
    }

    public static IEnumerable<(Int32 Position, Int32 Length)> CommentLines(this ReadOnlySpan<Char> source)
    {
        var comments = new List<(Int32 Position, Int32 Length)>();

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
                break;

            if (current < next && source.Slice(current, (next - current)).IsComment())
                comments.Add((current, (next - current)));

            current = next;

            while (    (current < source.Length)
                    && (source[current] == '\r' || source[current] == '\n'))
                current++;
        }

        return comments;
    }

}