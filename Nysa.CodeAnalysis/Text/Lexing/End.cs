using System;

namespace Nysa.Text;

public static class End
{
    /// <summary>
    /// Creates a new empty span at the end of a given string.
    /// </summary>
    /// <param name="forSource"></param>
    /// <returns></returns>
    public static TextSpan Span(String forSource)
        => new TextSpan(forSource, forSource.Length, 0);

}