using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nysa.CodeAnalysis.VbScript
{

    /// <summary>
    /// Contains data from a single stylesheet include element.
    /// </summary>
    /// <param name="Source">The value from the include element's "src" attribute.</param>
    /// <param name="Element">The xsl:include object that is the include element.</param>
    public record XslIncludeItem(
        String Source,
        XmlElement Element
    );

}
