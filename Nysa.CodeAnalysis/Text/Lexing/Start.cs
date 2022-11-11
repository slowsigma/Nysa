using System;

namespace Nysa.Text;

public static class Start
{
    /// <summary>
    /// Creates a new empty span at the start of a given string.
    /// </summary>
    /// <param name="forSource"></param>
    /// <returns></returns>
    public static TextSpan Span(String forSource)
        => new TextSpan(forSource, 0, 0);
    /// <summary>
    /// Creates a new empty span starting at the end of an existing span.
    /// </summary>
    /// <param name="aSpan"></param>
    /// <returns></returns>
    public static TextSpan SpanAfter(TextSpan aSpan)
        => new TextSpan(aSpan.Source, aSpan.End.Value, 0);

}