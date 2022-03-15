using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        private static String Path(this XElement element)
            => element.Parent == null
               ? element.Name.ToString()
               : String.Concat(element.Parent.Path(), "/", element.Name.ToString());
        private static String Path(this XElement @this, XAttribute attribute)
            => String.Concat(@this.Path(), "/@", attribute.Name.LocalName);

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
            (List<(String Source, XElement Element)> Includes, List<XHtmlVbScriptParse> VbParses) FromDocument(XDocument document)
            {
                var includes = new Dictionary<String, XElement>(StringComparer.OrdinalIgnoreCase);
                var parses   = new List<XHtmlVbScriptParse>();

                foreach (var script in document.Descendants().Where(e => e.Name.LocalName.DataEquals("script")))
                {
                    var lang = script.Attributes().FirstOrDefault(a => a.Name.LocalName.DataEndsWith("language"));
                    var type = script.Attributes().FirstOrDefault(a => a.Name.LocalName.DataEndsWith("type"));
                    var src  = script.Attributes().FirstOrDefault(a => a.Name.LocalName.DataEndsWith("src"));

                    if ((lang?.Value ?? String.Empty).DataEquals("vbscript") ||
                        (type?.Value ?? String.Empty).DataEquals(@"text/vbscript"))
                    {
                        if (!String.IsNullOrWhiteSpace(src?.Value))
                        {
                            if (!includes.ContainsKey(src.Value))
                                includes.Add(src.Value, script);
                        }
                        else if (!String.IsNullOrWhiteSpace(script.Value))
                        {
                            var vbParse = (new VbScriptContent(script.Path(), script.Value)).Parse();

                            parses.Add(new XHtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, script));
                        }
                    }
                }

                if (eventAttributes != null)
                {
                    foreach (var element in document.Descendants())
                    {
                        foreach (var attribute in element.Attributes().Where(a => eventAttributes.Contains(a.Name.LocalName)))
                        {
                            if (attribute.Value.DataStartsWith(_vbscript_colon))
                            {
                                var vbString = String.Concat(attribute.Value.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n");
                                var vbParse = (new VbScriptContent(element.Path(attribute), vbString)).Parse();

                                parses.Add(new XHtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, element, attribute));
                            }
                        }
                    }
                }

                return (includes.Select(kv => (Source: kv.Key, Element: kv.Value)).ToList(), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = XDocument.Parse(htmlContent.Value, LoadOptions.PreserveWhitespace);

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
                            if (!includes.ContainsKey(src.Value))
                                includes.Add(src.Value, script);
                        }
                        else if (!String.IsNullOrWhiteSpace(script.InnerText))
                        {
                            var vbParse = (new VbScriptContent(script.XPath, script.InnerText)).Parse();

                            parses.Add(new HtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, script));
                        }
                    }
                }

                if (eventAttributes != null)
                {
                    foreach (var node in document.DocumentNode.Descendants())
                    {
                        foreach (var attribute in node.Attributes.Where(a => eventAttributes.Contains(a.Name)))
                        {
                            if (attribute.Value.DataStartsWith(_vbscript_colon))
                            {
                                var attrValue = HtmlEntity.DeEntitize(attribute.Value);
                                var vbString = String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n");
                                var vbParse = (new VbScriptContent(attribute.XPath, vbString)).Parse();

                                parses.Add(new HtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, node, attribute));
                            }
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
            IEnumerable<XslVbScriptParse> FromDocument(XDocument document)
            {
                foreach (var script in document.Descendants().Where(d => d.Name.LocalName.Equals("script")))
                {
                    var lang = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("language"));
                    var pref = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("implements-prefix")).Map(a => a.Value);
                    var path = script.Path();

                    var parse = (new VbScriptContent(path, script.Value)).Parse();

                    if (lang.Map(a => a.Value.DataEquals("vbscript")).Or(false))
                        yield return new XslVbScriptParse(parse.Content, parse.SyntaxRoot, pref, script, null, false, null);
                }

                if (eventAttributes != null)
                {
                    foreach (var element in document.Descendants())
                    {

                        if (element.Name.NamespaceName.DataEquals(_xsl_namespace_uri) && element.Name.LocalName.DataEquals("attribute"))
                        {
                            var nameAttr = element.Attributes().FirstOrNone(a => a.Name.LocalName.DataEquals("name"));

                            if (nameAttr is Some<XAttribute> someNameAttr && eventAttributes.Contains(someNameAttr.Value.Value))
                            {
                                // at this point we have several possibilities
                                //   1. element contains all text with no sub-elements
                                //   2. element contains a mix of text and sub-elements and sub-elements are only xsl:value
                                //   3. element contains a mix of text and sub-elements and sub-elements are only xsl:value and xsl:text
                                //      if we know there is no text outside of xsl:text elements, then we might be able to take a translation
                                //      of the vbscript that contains substitutions (placeholders), and break that back down to xsl:text
                                //      using the substitution markers to know where to split up the translation

                                var build = new StringBuilder();
                                var xtxt  = false;
                                var xstxt = false;
                                var subs  = new List<(String name, XElement xslValue)>();
                                var subNo = 0;
                                var bail  = false;

                                foreach (var node in element.Nodes())
                                {
                                    if (node is XText xText)
                                    {
                                        xtxt = true;
                                        if (xstxt)
                                            bail = true;

                                        build.Append(xText.Value);
                                    }
                                    else if (node is XElement xslText && xslText.Name.NamespaceName.DataEquals(_xsl_namespace_uri) && xslText.Name.LocalName.DataEquals("text"))
                                    {
                                        xstxt = true;
                                        if (xtxt)
                                            bail = true;

                                        build.Append(xslText.Value);
                                    }
                                    else if (node is XElement xslValue && xslValue.Name.NamespaceName.DataEquals(_xsl_namespace_uri) && xslValue.Name.LocalName.DataEquals("value"))
                                    {
                                        var subName = $"xsl_value_placeholder_{++subNo}";
                                        build.Append(subName);
                                        subs.Add((subName, xslValue));
                                    }
                                    else if (node is XElement xOther)
                                    {
                                        bail = true;
                                    }

                                    if (bail)
                                        break;
                                }

                                if (!bail)
                                {
                                    var parseText = build.ToString();

                                    if (parseText.DataStartsWith(_vbscript_colon))
                                    {
                                        var vbString = String.Concat(parseText.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n");
                                        var vbParse = (new VbScriptContent(element.Path(), vbString)).Parse();
                                        yield return new XslVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, Option.None, element, null, xstxt, subs);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var attribute in element.Attributes().Where(a => eventAttributes.Contains(a.Name.LocalName)))
                            {
                                if (attribute.Value.DataStartsWith(_vbscript_colon))
                                {
                                    var attrValue = HtmlEntity.DeEntitize(attribute.Value);
                                    var vbString = String.Concat(attrValue.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n");
                                    var vbParse = (new VbScriptContent(element.Path(attribute), vbString)).Parse();

                                    yield return new XslVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, Option.None, element, attribute, false, null);
                                }
                            }
                        }
                    }
                }
            }

            return Return.Try(() =>
                                {
                                    var doc = XDocument.Parse(xslContent.Value, LoadOptions.PreserveWhitespace);

                                    return FromDocument(doc).Make(p => new XslParse(xslContent, doc, p));
                                });
        }

    }

}