using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

using Nysa.Text.Parsing;
using LexToken  = Nysa.Text.Lexing.Token;
using ParseNode = Nysa.Text.Parsing.Node;

using HtmlAgilityPack;

using Nysa.Logics;
using Nysa.Text;
using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript
{

    public static class ParseFunctions
    {
        public static readonly String XSL_VALUE_PLACEHOLDER_PREFIX = "xsl_value_placeholder_";

        private static readonly String _vbscript_colon = "vbscript:";
        private static readonly String _xsl_namespace_uri = "http://www.w3.org/1999/XSL/Transform";

        private static String Path(this XmlElement element)
            => element.ParentNode == null
               ? element.Name.ToString()
               : element.ParentNode is XmlElement parentElem
                 ? String.Concat(parentElem.Path(), "/", element.Name.ToString())
                 : String.Empty;

        private static String Path(this XmlElement @this, XmlAttribute attribute)
            => String.Concat(@this.Path(), "/@", attribute.LocalName);

        private static Suspect<ParseNode> Parse(this String @this)
            => Return.Try(() => VbScript.Language.Parse(String.Concat(@this, "\r\n")));

        private static HtmlContent AsHtmlContent(this XmlContent @this)
            => new HtmlContent(@this.Source, @this.Hash, @this.Value);

        public static Suspect<Parse> Parse(this Content content, IReadOnlySet<String>? eventAttributes = null)
            =>   content is HtmlContent      html     ? html.Parse(eventAttributes).Map(s => (Parse)s)
               : content is VbScriptContent  vbScript ? vbScript.Parse().Confirmed().Map(s => (Parse)s)
               : content is XslContent       xsl      ? xsl.Parse(eventAttributes).Map(s => (Parse)s)
               : content is XmlContent       xml      ? xml.AsHtmlContent().Parse(eventAttributes).Map(s => (Parse)s)
               :                                        throw new ArgumentException("Parse does not accept content of this type.");

        public static Suspect<XmlParse> Parse(this XmlContent content, Func<XmlDocument, IEnumerable<(XmlElement Element, XmlAttribute? Attribute, String Script)>> selector)
        {
            List<XmlVbScriptParse> FromDocument(XmlDocument document)
            {
                var parses = new List<XmlVbScriptParse>();

                foreach (var (element, attribute, script) in selector(document))
                {
                    if (!String.IsNullOrWhiteSpace(script))
                    {
                        var section = new VbScriptSection(element.Path(), script);
                        var vbParse = section.Parse();

                        parses.Add(new XmlVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, element, attribute));
                    }
                }

                return parses;
            }

            return Return.Try(() =>
                               {
                                   var doc = new XmlDocument();
                                   doc.PreserveWhitespace = true;
                                   doc.LoadXml(content.Value);

                                   return FromDocument(doc).Make(t => new XmlParse(content, doc, t));
                               });
        }

        private static Suspect<XHtmlParse> ParseXml(this HtmlContent htmlContent, IReadOnlySet<String>? eventAttributes)
        {
            (IEnumerable<XHtmlIncludeItem> Includes, List<XHtmlVbScriptParse> VbParses) FromDocument(XmlDocument document)
            {
                var includes    = new Dictionary<String, XmlElement>(StringComparer.OrdinalIgnoreCase);
                var parses      = new List<XHtmlVbScriptParse>();
                var descendants = document.SelectNodes("//*");

                if (descendants != null)
                {
                    foreach (var script in descendants.Cast<XmlNode>().Where(n => n is XmlElement).Cast<XmlElement>())
                    {
                        var lang = script.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("language"));
                        var type = script.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("type"));
                        var src  = script.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("src"));

                        if (   lang.Map(a => a.Value.DataEquals("vbscript")).Or(false)
                            || type.Map(a => a.Value.DataEquals(@"text/vbscript")).Or(false))
                        {
                            if (src is Some<XmlAttribute> someSrcAttr)
                            {
                                var srcValue = HttpUtility.UrlDecode(someSrcAttr.Value.Value);

                                if (!includes.ContainsKey(srcValue))
                                    includes.Add(srcValue, script);
                            }
                            else if (!String.IsNullOrWhiteSpace(script.InnerText))
                            {
                                var section = new VbScriptSection(script.Path(), script.InnerText);
                                var vbParse = section.Parse();

                                parses.Add(new XHtmlVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, script));
                            }
                        }
                    }

                    if (eventAttributes != null)
                    {
                        foreach (var element in descendants.Cast<XmlNode>().Where(n => n is XmlElement).Cast<XmlElement>())
                        {
                            foreach (var attribute in element.Attributes.Cast<XmlAttribute>().Where(a => eventAttributes.Contains(a.LocalName)))
                            {
                                var attrValue = HtmlEntity.DeEntitize(attribute.Value);
                                
                                var vbString  = attrValue.DataStartsWith(_vbscript_colon)
                                                ? String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n")
                                                : String.Concat(attrValue.Replace("'", "\""), "\r\n");

                                var section   = new VbScriptSection(element.Path(attribute), vbString);
                                var vbParse   = section.Parse();

                                parses.Add(new XHtmlVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, element, attribute));
                            }
                        }
                    }
                }

                return (includes.Select(kv => new XHtmlIncludeItem(kv.Key, kv.Value)), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = new XmlDocument();
                                   doc.PreserveWhitespace = true;
                                   doc.LoadXml(htmlContent.Value);

                                   return FromDocument(doc).Make(t => new XHtmlParse(htmlContent, doc, t.Includes, t.VbParses));
                               });
        }

        private static Suspect<HtmlParse> ParseHtml(this HtmlContent htmlContent, IReadOnlySet<String>? eventAttributes)
        {
            (IEnumerable<HtmlIncludeItem> Includes, List<HtmlVbScriptParse> Parses) FromDocument(HtmlDocument document)
            {
                var includes = new Dictionary<String, HtmlNode>(StringComparer.OrdinalIgnoreCase);
                var parses   = new List<HtmlVbScriptParse>();

                foreach (var script in document.DocumentNode.Descendants("script"))
                {
                    var lang = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("language"));
                    var type = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("type"));
                    var src  = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("src"));

                    if ((lang?.Value ?? String.Empty).DataEquals("vbscript") ||
                        (type?.Value ?? String.Empty).DataEquals(@"text/vbscript"))
                    {
                        if (!String.IsNullOrWhiteSpace(src?.Value))
                        {
                            var srcValue = HttpUtility.UrlDecode(src.Value);

                            if (!includes.ContainsKey(srcValue))
                                includes.Add(srcValue, script);
                        }
                        else if (!String.IsNullOrWhiteSpace(script.InnerText))
                        {
                            var section = new VbScriptSection(script.XPath, script.InnerText);
                            var vbParse = section.Parse();

                            parses.Add(new HtmlVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, script));
                        }
                    }
                }

                if (eventAttributes != null)
                {
                    foreach (var node in document.DocumentNode.Descendants())
                    {
                        foreach (var attribute in node.Attributes.Where(a => eventAttributes.Contains(a.Name)))
                        {
                            var attrValue = HtmlEntity.DeEntitize(attribute.Value);

                            var vbString  = attrValue.DataStartsWith(_vbscript_colon)
                                            ? String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n")
                                            : String.Concat(attrValue.Replace("'", "\""), "\r\n");

                            var section   = new VbScriptSection(attribute.XPath, vbString);
                            var vbParse   = section.Parse();

                            parses.Add(new HtmlVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, node, attribute));
                        }
                    }
                }

                return (includes.Select(kv => new HtmlIncludeItem(kv.Key, Node: kv.Value)), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = new HtmlDocument();
                                   doc.LoadHtml(htmlContent.Value);
                                   return FromDocument(doc).Make(t => new HtmlParse(htmlContent, doc, t.Includes, t.Parses));
                               });
        }

        public static VbScriptParse Parse(this VbScriptContent @this)
            => @this.Value
                    .Parse()
                    .Make(p => new VbScriptParse(@this, p, p.ToProgram()));

        public static Suspect<Parse> Parse(this HtmlContent @this, IReadOnlySet<String>? eventAttributes = null)
            => @this.ParseXml(eventAttributes)
                    .Match(xp => xp.Confirmed<Parse>(),
                           e => @this.ParseHtml(eventAttributes).Bind(hp => hp.Confirmed<Parse>()));

        private static Boolean IsXslElement(this XmlElement @this, String elementName)
            => @this.NamespaceURI.DataEquals(_xsl_namespace_uri) && @this.LocalName.DataEquals(elementName);

        private static Boolean IsXslElement(this XmlNode @this, String elementName)
            => @this is XmlElement elem && elem.NamespaceURI.DataEquals(_xsl_namespace_uri) && elem.LocalName.DataEquals(elementName);

        private static (Int32 XslTextCount, Int32 ValueOfCount, Int32 OthersCount) ChildXslCounts(this XmlElement @this)
        {
            var xslTexts    = 0;
            var xslValueOfs = 0;
            var xslOthers   = 0;

            foreach (var elem in @this.ChildNodes.Cast<XmlNode>().Select(n => n is XmlElement elem ? elem.Some() : Option<XmlElement>.None).SomeOnly())
            {
                if (elem.IsXslElement("text"))
                    xslTexts++;
                else if (elem.IsXslElement("value-of"))
                    xslValueOfs++;
                else if (elem.NamespaceURI.DataEquals(_xsl_namespace_uri))
                    xslOthers++;
            }

            return (xslTexts, xslValueOfs, xslOthers);
        }

        // Gets parses for descendant elements that could have 'vbscript:...' text for parsing.
        // This is only intended for xsl:attribute elements that are event handlers where the
        // element children are not text, xsl:text, and xsl:value-of only.
        private static IReadOnlyList<XslVbScriptParse> DescendantParses(this XmlElement @this)
        {
            var results = new List<XslVbScriptParse>();

            foreach (var child in @this.ChildNodes)
            {
                if (child is XmlElement elem)
                    results.AddRange(elem.DescendantParses());
                else if (child is XmlText text && !String.IsNullOrWhiteSpace(text.Value))
                {
                    var vbsHead = text.Value.DataStartsWith(_vbscript_colon);
                    var rawText = String.Concat((vbsHead ? text.Value.Substring(_vbscript_colon.Length) : text.Value), "\r\n");
                    var section = new VbScriptSection(@this.Path(), rawText);
                    var vbParse = section.Parse();

                    if (vbsHead || vbParse.SyntaxRoot is Confirmed<ParseNode>)
                        results.Add(new XslVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, Option.None, @this, null, null));
                }
            }

            return results;
        }

        private static Option<XslVbScriptParse> XslAttributeParse(this XmlElement @this, Int32 xslTextsCount, XmlDocument document)
        {
            var build    = new StringBuilder();
            var contents = new List<(XmlNode TextOrPlaceholder, XmlElement? xslValueOf)>();
            var placeNum = 0;

            foreach (var node in @this.ChildNodes.Cast<XmlNode>())
            {
                if (xslTextsCount == 0 && node is XmlText xText) // ignore all XmlText nodes if we have xslText elements
                {
                    build.Append(xText.Value);
                    contents.Add((node, null));
                }
                else if (node is XmlElement xslText && xslText.IsXslElement("text"))
                {
                    build.Append(xslText.InnerText);
                    contents.Add((node, null));
                }
                else if (node is XmlElement xslValueOf && xslValueOf.IsXslElement("value-of"))
                {
                    var subName = $"{XSL_VALUE_PLACEHOLDER_PREFIX}{++placeNum}";
                    build.Append(subName);
                    
                    contents.Add((document.CreateTextNode(subName), xslValueOf));
                }
                else if (node is XmlElement xOther)
                {
                    return Option.None;
                }
            }

            var rawText = String.Concat(build.ToString().Replace("'", "\""), "\r\n");
            var vbsHead = rawText.DataStartsWith(_vbscript_colon);
            var section = new VbScriptSection(@this.Path(), vbsHead ? rawText.Substring(_vbscript_colon.Length) : rawText);
            var vbParse = section.Parse();

            return (vbsHead || vbParse.SyntaxRoot is Confirmed<ParseNode>)
                   ? (new XslVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, Option.None, @this, null, contents)).Some()
                   : Option.None;
        }

        public static Suspect<XslParse> Parse(this XslContent xslContent, IReadOnlySet<String>? eventAttributes = null)
        {
            (IEnumerable<XslIncludeItem> Includes, IEnumerable<XslVbScriptParse> Parses) FromDocument(XmlDocument document)
            {
                var includes    = new Dictionary<String, XmlElement>(StringComparer.OrdinalIgnoreCase);
                var parses      = new List<XslVbScriptParse>();
                var descendants = document.SelectNodes("//*");

                if (descendants != null)
                {
                    foreach (var script in descendants.Cast<XmlNode>().Where(d => d is XmlElement elem && elem.LocalName.Equals("script")).Cast<XmlElement>())
                    {
                        var lang = script.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("language"));
                        var pref = script.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("implements-prefix"));
                        var path = script.Path();

                        var section = new VbScriptSection(path, script.InnerText);
                        var parse   = section.Parse();

                        if (lang.Map(a => a.Value.DataEquals("vbscript")).Or(false))
                            parses.Add(new XslVbScriptParse(section, parse.SyntaxRoot, parse.SemanticRoot, pref.Map(a => a.Value), script, null, null));
                    }

                    if (eventAttributes != null)
                    {
                        foreach (var element in descendants.Cast<XmlNode>().Where(n => n is XmlElement).Cast<XmlElement>())
                        {

                            if (element.IsXslElement("attribute"))
                            {
                                var nameAttr = element.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("name"));

                                if (nameAttr is Some<XmlAttribute> someNameAttr && eventAttributes.Contains(someNameAttr.Value.Value))
                                {
                                    var (xslTexts,
                                         xslValueOfs,
                                         xslOthers   ) = element.ChildXslCounts();

                                    // all possibilities
                                    //   xslTexts | xslValueOfs | xslOthers | ???
                                    //       0    |      0      |     0     | parse element.innerText
                                    //      !0    |      0      |     0     | parse concatenation of all xsl:text
                                    //       0    |     !0      |     0     | parse a concatenation of all xmlText with placeholder for each value-of
                                    //      !0    |     !0      |     0     | parse a concatenation of all xsl:text with placeholder for each value-of
                                    //       x    |      x      |    !0     | parse each descendent with text starting with "vbscript:"

                                    if (xslOthers > 0)
                                        parses.AddRange(element.DescendantParses());
                                    else if (element.XslAttributeParse(xslTexts, document) is Some<XslVbScriptParse> someParse)
                                        parses.Add(someParse.Value);
                                }
                            }
                            else if (   element.IsXslElement("include")
                                     || element.IsXslElement("import"))
                            {
                                var href = element.GetAttribute("href");

                                if (!String.IsNullOrWhiteSpace(href))
                                {
                                    var hrefValue = HttpUtility.UrlDecode(href);

                                    if (!includes.ContainsKey(hrefValue))
                                        includes.Add(hrefValue, element);
                                }
                            }
                            else // check element for actual event attributes (i.e., not xsl:attribute)
                            {
                                foreach (var attribute in element.Attributes.Cast<XmlAttribute>().Where(a => eventAttributes.Contains(a.LocalName)))
                                {
                                    var attrValue = HtmlEntity.DeEntitize(attribute.Value);
                                    var vbString  = attrValue.DataStartsWith(_vbscript_colon)
                                                    ? String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n")
                                                    : String.Concat(attrValue.Replace("'", "\""), "\r\n");
                                    var section   = new VbScriptSection(element.Path(attribute), vbString);
                                    var vbParse   = section.Parse();

                                    parses.Add(new XslVbScriptParse(section, vbParse.SyntaxRoot, vbParse.SemanticRoot, Option.None, element, attribute, null));
                                }
                            }
                        }
                    }
                }

                return (includes.Select(kv => new XslIncludeItem(kv.Key, kv.Value)), parses);
            }

            return Return.Try(() =>
                                {
                                    var doc = new XmlDocument();
                                    doc.PreserveWhitespace = true;
                                    doc.LoadXml(xslContent.Value);

                                    return FromDocument(doc).Make(t => new XslParse(xslContent, doc, t.Includes, t.Parses));
                                });
        }

    }

}