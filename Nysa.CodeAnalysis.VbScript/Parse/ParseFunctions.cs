using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

using Nysa.Logics;
using Nysa.Text;

using Dorata.Text.Parsing;
using LexToken  = Dorata.Text.Lexing.Token;
using ParseNode = Dorata.Text.Parsing.Node;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public static class ParseFunctions
    {
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

        private static ParseException ToParseError(this Dorata.Text.Parsing.VBScript.ParseError @this)
            => new ParseException(@this.Type == VBScript.ParseErrorTypes.InvalidSymbol ? "Invalid symbol." : "Unexpected symbol.",
                                  @this.LineNumber,
                                  @this.ColumnNumber,
                                  @this.ErrorLine,
                                  @this.ErrorRules.Select(l => String.Join("\r\n", l), String.Empty));

        private static Suspect<ParseNode> Parse(this String @this)
            => Return.Try(() => VBScript.Parse(String.Concat(@this, "\r\n")))
                     .Bind(g => g.IsA
                                ? g.A.Confirmed()
                                : (Failed<ParseNode>)g.B.ToParseError());

        public static Suspect<Parse> Parse(this Content content, HashSet<String>? eventAttributes = null)
            =>   content is HtmlContent      html     ? html.Parse(eventAttributes).Map(s => (Parse)s)
               : content is VbScriptContent  vbScript ? vbScript.Parse().Confirmed().Map(s => (Parse)s)
               : content is XslContent       xsl      ? xsl.Parse(eventAttributes).Map(s => (Parse)s)
               :                                        throw new ArgumentException("Parse does not accept content of this type.");

        private static Suspect<XHtmlParse> ParseXml(this HtmlContent htmlContent, HashSet<String>? eventAttributes)
        {
            (List<(String Source, XmlElement Element)> Includes, List<XHtmlVbScriptParse> VbParses) FromDocument(XmlDocument document)
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

                                parses.Add(new XHtmlVbScriptParse(section, vbParse.SyntaxRoot, script));
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

                                parses.Add(new XHtmlVbScriptParse(section, vbParse.SyntaxRoot, element, attribute));
                            }
                        }
                    }
                }

                return (includes.Select(kv => (Source: kv.Key, Element: kv.Value)).ToList(), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = new XmlDocument();
                                   doc.PreserveWhitespace = true;
                                   doc.LoadXml(htmlContent.Value);

                                   return FromDocument(doc).Make(t => new XHtmlParse(htmlContent, doc, t.Includes, t.VbParses));
                               });
        }

        private static Suspect<HtmlParse> ParseHtml(this HtmlContent htmlContent, HashSet<String>? eventAttributes)
        {
            (List<(String Soure, HtmlNode Node)> Includes, List<HtmlVbScriptParse> Parses) FromDocument(HtmlDocument document)
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

                            parses.Add(new HtmlVbScriptParse(section, vbParse.SyntaxRoot, script));
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

                            parses.Add(new HtmlVbScriptParse(section, vbParse.SyntaxRoot, node, attribute));
                        }
                    }
                }

                return (includes.Select(kv => (Source: kv.Key, Node: kv.Value)).ToList(), parses);
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
                    .Make(p => new VbScriptParse(@this, p));

        public static Suspect<Parse> Parse(this HtmlContent @this, HashSet<String>? eventAttributes = null)
            => @this.ParseXml(eventAttributes)
                    .Match(xp => xp.Confirmed<Parse>(),
                           e => @this.ParseHtml(eventAttributes).Bind(hp => hp.Confirmed<Parse>()));

        public static Suspect<XslParse> Parse(this XslContent xslContent, HashSet<String>? eventAttributes = null)
        {
            IEnumerable<XslVbScriptParse> FromDocument(XmlDocument document)
            {
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
                            yield return new XslVbScriptParse(section, parse.SyntaxRoot, pref.Map(a => a.Value), script, null, null);
                    }

                    if (eventAttributes != null)
                    {
                        foreach (var element in descendants.Cast<XmlNode>().Where(n => n is XmlElement).Cast<XmlElement>())
                        {

                            if (element.NamespaceURI.DataEquals(_xsl_namespace_uri) && element.LocalName.DataEquals("attribute"))
                            {
                                var nameAttr = element.Attributes.Cast<XmlAttribute>().FirstOrNone(a => a.LocalName.DataEquals("name"));

                                if (nameAttr is Some<XmlAttribute> someNameAttr && eventAttributes.Contains(someNameAttr.Value.Value))
                                {
                                    // at this point we have several possibilities
                                    //   1. element contains all text with no sub-elements
                                    //   2. element contains a mix of text and sub-elements and sub-elements are only xsl:value
                                    //   3. element contains a mix of text and sub-elements and sub-elements are only xsl:value and xsl:text
                                    //      if we know there is no text outside of xsl:text elements, then we might be able to take a translation
                                    //      of the vbscript that contains substitutions (placeholders), and break that back down to xsl:text
                                    //      using the substitution markers to know where to split up the translation

                                    var build = new StringBuilder();
                                    var conts = new List<(XmlNode TextOrPlaceholder, XmlElement? xslValueOf)>();
                                    var xsTxt = element.ChildNodes
                                                       .Cast<XmlNode>()
                                                       .Where(n => n is XmlElement elem && elem.NamespaceURI.DataEquals(_xsl_namespace_uri) && elem.LocalName.DataEquals("text"))
                                                       .Count();
                                    var plhNo = 0;
                                    var bail  = false;

                                    foreach (var node in element.ChildNodes.Cast<XmlNode>())
                                    {
                                        if (xsTxt == 0 && node is XmlText xText) // ignore all XmlText nodes if we have xslText elements
                                        {
                                            build.Append(xText.Value);
                                            conts.Add((node, null));
                                        }
                                        else if (node is XmlElement xslText && xslText.NamespaceURI.DataEquals(_xsl_namespace_uri) && xslText.LocalName.DataEquals("text"))
                                        {
                                            build.Append(xslText.InnerText);
                                            conts.Add((node, null));
                                        }
                                        else if (node is XmlElement xslValueOf && xslValueOf.NamespaceURI.DataEquals(_xsl_namespace_uri) && xslValueOf.LocalName.DataEquals("value-of"))
                                        {
                                            var subName = $"xsl_value_placeholder_{++plhNo}";
                                            build.Append(subName);
                                            
                                            conts.Add((document.CreateTextNode(subName), xslValueOf));
                                        }
                                        else if (node is XmlElement xOther)
                                        {
                                            bail = true;
                                            break;
                                        }
                                    }

                                    if (!bail)
                                    {
                                        var parseText = build.ToString();

                                        if (parseText.DataStartsWith(_vbscript_colon))
                                        {
                                            var vbString = String.Concat(parseText.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n");
                                            var section  = new VbScriptSection(element.Path(), vbString);
                                            var vbParse  = section.Parse();
                                            yield return new XslVbScriptParse(section, vbParse.SyntaxRoot, Option.None, element, null, conts);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var attribute in element.Attributes.Cast<XmlAttribute>().Where(a => eventAttributes.Contains(a.LocalName)))
                                {
                                    var attrValue = HtmlEntity.DeEntitize(attribute.Value);
                                    var vbString  = attrValue.DataStartsWith(_vbscript_colon)
                                                    ? String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n")
                                                    : String.Concat(attrValue.Replace("'", "\""), "\r\n");
                                    var section   = new VbScriptSection(element.Path(attribute), vbString);
                                    var vbParse   = section.Parse();

                                    yield return new XslVbScriptParse(section, vbParse.SyntaxRoot, Option.None, element, attribute, null);
                                }
                            }
                        }
                    }
                }
            }

            return Return.Try(() =>
                                {
                                    var doc = new XmlDocument();
                                    doc.PreserveWhitespace = true;
                                    doc.LoadXml(xslContent.Value);

                                    return FromDocument(doc).Make(p => new XslParse(xslContent, doc, p));
                                });
        }

    }

}