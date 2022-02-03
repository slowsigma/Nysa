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

        public static Suspect<Parse> Parse(this Content content)
            =>   content is HtmlContent      html     ? html.Parse().Map(s => (Parse)s)
               : content is VbScriptContent  vbScript ? vbScript.Parse().Confirmed().Map(s => (Parse)s)
               : content is XslContent       xsl      ? xsl.Parse().Map(s => (Parse)s)
               :                                        throw new ArgumentException("Parse does not accept content of this type.");

        public static VbScriptParse Parse(this VbScriptContent @this)
            => @this.Value
                    .Parse()
                    .Make(p => new VbScriptParse(@this, p));

        public static Suspect<HtmlParse> Parse(this HtmlContent htmlContent)
        {
            (HtmlDocument Document, List<HtmlVbScriptInclude> includes, List<HtmlVbScriptParse> VbParses) FromDocument(HtmlDocument document)
            {
                var includes = new Dictionary<String, HtmlVbScriptInclude>(StringComparer.OrdinalIgnoreCase);
                var vbParses = new List<HtmlVbScriptParse>();

                foreach (var script in document.DocumentNode.Descendants("script"))
                {
                    var lang = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("language"));
                    var type = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("type"));
                    var src  = script.Attributes.FirstOrDefault(a => a.Name.DataEndsWith("src"));
                    var path = script.XPath;
                    var elem = Return.Try(() => XElement.Parse(script.OuterHtml));

                    if ((lang?.Value ?? String.Empty).DataEquals("vbscript") ||
                        (type?.Value ?? String.Empty).DataEquals(@"text/vbscript"))
                    {
                        if (src != null && !String.IsNullOrWhiteSpace(src.Value))
                        {
                            if (!includes.ContainsKey(src.Value))
                                includes.Add(src.Value, new HtmlVbScriptInclude(path, src.Value));
                        }
                        else if (elem is Confirmed<XElement> confirmed && !String.IsNullOrWhiteSpace(confirmed.Value.Value))
                        {
                            var vbParse = (new VbScriptContent(htmlContent.Source, confirmed.Value.Value, path.Some())).Parse();

                            vbParses.Add(new HtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot));
                        }
                        else if (elem is Confirmed<XElement>)
                        {
                            // skip, the element is commented out
                        }
                        else if (!String.IsNullOrWhiteSpace(script.InnerText))
                        {
                            var vbParse = (new VbScriptContent(htmlContent.Source, script.InnerText, path.Some())).Parse();

                            vbParses.Add(new HtmlVbScriptParse(vbParse.Content, vbParse.SyntaxRoot));
                        }
                    }
                }

                return (document, includes.Values.ToList(), vbParses);
            }

            return Return.Try(() =>
                               {
                                   var doc = new HtmlDocument();
                                   doc.LoadHtml(htmlContent.Value);
                                   return FromDocument(doc).Make(t => new HtmlParse(htmlContent, t.Document, t.includes, t.VbParses));
                               });
        }

        private static String Path(this XElement element)
            => element.Parent == null
               ? element.Name.ToString()
               : String.Concat(element.Parent.Path(), "/", element.Name.ToString());

        public static Suspect<XslParse> Parse(this XslContent xslContent)
        {
            IEnumerable<XslVbScriptParse> ParseOver(XElement root)
            {
                foreach (var script in root.DescendantsAndSelf().Where(d => d.Name.LocalName.Equals("script")))
                {
                    var lang = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("language"));
                    var pref = script.Attributes().FirstOrNone(a => a.Name.LocalName.DataEndsWith("implements-prefix")).Map(a => a.Value);
                    var path = script.Path();
                    var cont = new VbScriptContent(xslContent.Source, script.Value, path.Some());

                    if (lang.Match(a => a.Value.DataEquals("vbscript"), false))
                        yield return new XslVbScriptParse(cont, pref, cont.Value.Parse());
                }
            }

            return Return.Try(() => XElement.Parse(xslContent.Value))
                         .Map(r => new XslParse(xslContent, r, ParseOver(r)));
        }

    }

}