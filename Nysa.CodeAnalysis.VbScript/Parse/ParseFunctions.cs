using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            =>   content is HtmlContent      html     ? html.Parse().Map(s => (Parse)s)
               : content is VbScriptContent  vbScript ? vbScript.Parse().Confirmed().Map(s => (Parse)s)
               : content is XslContent       xsl      ? xsl.Parse().Map(s => (Parse)s)
               :                                        throw new ArgumentException("Parse does not accept content of this type.");

        public static VbScriptParse Parse(this VbScriptContent @this)
            => @this.Value
                    .Parse()
                    .Make(p => new VbScriptParse(@this, p));

        private static Suspect<HtmlXmlParse> ParseXml(this HtmlContent htmlContent, HashSet<String>? eventAttributes)
        {
            (XDocument Document, List<(String Source, XElement Element)> Includes, List<HtmlXmlVbScriptParse> VbParses) FromDocument(XDocument document)
            {
                var includes = new Dictionary<String, XElement>(StringComparer.OrdinalIgnoreCase);
                var parses   = new List<HtmlXmlVbScriptParse>();

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

                            parses.Add(new HtmlXmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, script));
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
                                var vbParse = (new VbScriptContent(element.Path(attribute), String.Concat(attribute.Value.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n"))).Parse();

                                parses.Add(new HtmlXmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, element, attribute));
                            }
                        }
                    }
                }

                return (document, includes.Select(kv => (Source: kv.Key, Element: kv.Value)).ToList(), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = XDocument.Parse(htmlContent.Value, LoadOptions.PreserveWhitespace);

                                   return FromDocument(doc).Make(t => new HtmlXmlParse(htmlContent, t.Document, t.Includes, t.VbParses));
                               });
        }

        public static Suspect<HtmlParse> ParseHtml(this HtmlContent htmlContent, HashSet<String>? eventAttributes)
        {
            (HtmlDocument Document, List<(String Soure, HtmlNode Node)> Includes, List<HtmlVbScriptParse> Parses) FromDocument(HtmlDocument document)
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
                                var vbParse = (new VbScriptContent(attribute.XPath, String.Concat(attribute.Value.Substring(_vbscript_colon.Length).Replace("'", "\""), "\r\n"))).Parse();

                                parses.Add(new HtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot, node, attribute));
                            }
                        }
                    }
                }

                return (document, includes.Select(kv => (Source: kv.Key, Node: kv.Value)).ToList(), parses);
            }

            return Return.Try(() =>
                               {
                                   var doc = new HtmlDocument();
                                   doc.LoadHtml(htmlContent.Value);
                                   return FromDocument(doc).Make(t => new HtmlParse(htmlContent, t.Document, t.Includes, t.Parses));
                               });
        }

        public static Suspect<Parse> Parse(this HtmlContent @this, HashSet<String>? eventAttributes = null)
            => @this.ParseXml(eventAttributes)
                    .Match(xp => xp.Confirmed<Parse>(),
                           e => @this.ParseHtml(eventAttributes).Bind(hp => hp.Confirmed<Parse>()));

        public static Suspect<XslParse> Parse(this XslContent xslContent)
        {
            IEnumerable<XslVbScriptParse> ParseOver(XElement root)
            {
                foreach (var script in root.DescendantsAndSelf().Where(d => d.Name.LocalName.Equals("script")))
                {
                    var lang = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("language"));
                    var pref = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("implements-prefix")).Map(a => a.Value);
                    var path = script.Path();

                    var cont = new VbScriptContent(path, script.Value);

                    if (lang.Match(a => a.Value.DataEquals("vbscript"), false))
                        yield return new XslVbScriptParse(cont, pref, cont.Value.Parse());
                }
            }

            return Return.Try(() => XElement.Parse(xslContent.Value))
                         .Map(r => new XslParse(xslContent, r, ParseOver(r)));
        }

    }

}