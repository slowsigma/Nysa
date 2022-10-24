using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    /// <summary>
    /// Contains data from a single Html page include tag.
    /// </summary>
    /// <param name="Source">The value from the include tag's "src" attribute.</param>
    /// <param name="Node">The HtmlNode object that is the include tag.</param>
    public record HtmlIncludeItem(
        String Source,
        HtmlNode Node
    );

}
