using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Nysa.Logics;
using Nysa.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript;

public static class SymbolFunctions
{


    private static String NamespacePrefix(this XmlElement? @this)
        => (@this?.GetPrefixOfNamespace("http://www.w3.org/1999/XSL/Transform") ?? String.Empty);

    public static IEnumerable<VariableSymbol> PageSymbols(this IEnumerable<XslParse> @this)
        => @this.Where(p => (p.Document.DocumentElement?.Name ?? String.Empty).DataEndsWith(":stylesheet"))
                .Select(p => (XslPrefix: p.Document.DocumentElement.NamespacePrefix(),
                              Elements:  p.Document?
                                          .SelectNodes("//*")?
                                          .OfType<XmlElement>() ?? None<XmlElement>.Enumerable()))
                .SelectMany(t => t.Elements
                                  .Where(e => e.NamespacePrefix() != t.XslPrefix && !String.IsNullOrWhiteSpace(e.GetAttribute("id")))
                                  .Select(e => new VariableSymbol(e.GetAttribute("id"), Option.None, Option.None, true, e.LocalName.Some(), SymbolCategories.page)));


    public static PageSymbols PageSymbols(this XHtmlParse @this, IEnumerable<XslParse> xslReferences)
        => new PageSymbols(@this.Document
                                .SelectNodes("//*")?
                                .OfType<XmlElement>()?
                                .Where(e => !String.IsNullOrWhiteSpace(e.GetAttribute("id")))
                                .Select(e => new VariableSymbol(e.GetAttribute("id"), Option.None, Option.None, true, e.LocalName.Some(), SymbolCategories.page))
                                .Concat(xslReferences.PageSymbols())
                           ?? None<VariableSymbol>.Enumerable());
    
}