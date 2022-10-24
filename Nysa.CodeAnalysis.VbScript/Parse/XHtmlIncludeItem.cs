using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nysa.CodeAnalysis.VbScript
{

    /// <summary>
    /// Contains data from a single XHtml page include element.
    /// </summary>
    /// <param name="Source">The value from the include element's "src" attribute.</param>
    /// <param name="Element">The HtmlNode object that is the include tag.</param>
    public record XHtmlIncludeItem(
        String Source,
        XmlElement Element
    );

}
