using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text.Parsing;
using Nysa.Text.Lexing;


using Nysa.CodeAnalysis.VbScript;
namespace Nysa.CodeAnalysis.Testing;

using Dorata.Text.Lexing;
//using Dorata.Text.Parsing;
using DorataVBScript = Dorata.Text.Parsing.VBScript;

public static class Tests
{
    private static String[] _DefaultEventAttributeNames = new String[]
    {
        "onafterprint",
        "onbeforeprint",
        "onbeforeunload",
        "onerror",
        "onhashchange",
        "onload",
        "onmessage",
        "onoffline",
        "ononline",
        "onpagehide",
        "onpageshow",
        "onpopstate",
        "onresize",
        "onstorage",
        "onunload",
        "onblur",
        "onchange",
        "oncontextmenu",
        "onfocus",
        "oninput",
        "oninvalid",
        "onreset",
        "onsearch",
        "onselect",
        "onsubmit",
        "onkeydown",
        "onkeypress",
        "onkeyup",
        "onclick",
        "ondblclick",
        "onmousedown",
        "onmousemove",
        "onmouseout",
        "onmouseover",
        "onmouseup",
        "onmousewheel",
        "onwheel",
        "ondrag",
        "ondragend",
        "ondragenter",
        "ondragleave",
        "ondragover",
        "ondragstart",
        "ondrop",
        "onscroll",
        "oncopy",
        "oncut",
        "onpaste",
        "onabort",
        "oncanplay",
        "oncanplaythrough",
        "oncuechange",
        "ondurationchange",
        "onemptied",
        "onended",
        "onerror",
        "onloadeddata",
        "onloadedmetadata",
        "onloadstart",
        "onpause",
        "onplay",
        "onplaying",
        "onprogress",
        "onratechange",
        "onseeked",
        "onseeking",
        "onstalled",
        "onsuspend",
        "ontimeupdate",
        "onvolumechange",
        "onwaiting",
        "ontoggle"
    };        

    public static void XslParseTest()
    {
        var events = _DefaultEventAttributeNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var testfile = @"C:\Git\cj-odyssey_mainline\Webs\CaseManagement\AttorneyManager\Charges\XML\tabProsecutorCharges.xsl";

        if (testfile.ToContent() is Suspect<Content> suspect && suspect is Confirmed<Content> goodContent && goodContent.Value is XslContent xslContent)
        {
            var parse = xslContent.Parse(events);
        }
    }

    public static void VbScriptParseTest(String scriptsFolder)
    {
        foreach (var file in Directory.EnumerateFiles(scriptsFolder, "*.vbs", SearchOption.TopDirectoryOnly))
        {
            var parse = VBScript.Parse(File.ReadAllText(file));

            if (parse is Confirmed<Node> root)
            {

            }
            else
            {
                parse.Affect(c => {},
                                e => { Console.WriteLine(e.ToString()); });
            }
        }
    }

    public static void LexingTestDorata(String scriptsFolder)
    {
        var tklFile = Path.Combine(scriptsFolder, "FullTokenList-DorataLexer.txt");
        
        if (File.Exists(tklFile))
            File.Delete(tklFile);

        using (var stream = File.OpenWrite(tklFile))
        using (var writer = new StreamWriter(stream))
        {
            foreach (var file in Directory.EnumerateFiles(scriptsFolder, "*.vbs", SearchOption.TopDirectoryOnly))
            {
                var source = File.ReadAllText(file);

                var hits = DorataVBScript.Seek.Repeat(source).Where(h => h.Id != Dorata.Text.Identifier.Trivia).ToArray();

                writer.WriteLine(file);

                foreach (var hit in hits)
                {
                    var value = hit.Span.Value;

                    if (String.IsNullOrWhiteSpace(value))
                        writer.WriteLine($"Id: {DorataVBScript.Grammar.Symbol(hit.Id)} - white-space");
                    else
                        writer.WriteLine($"Id: {DorataVBScript.Grammar.Symbol(hit.Id)} - {value}");
                }
            }

            writer.Flush();
            writer.Close();
        }
    }



    public static void LexingTest(String scriptsFolder)
    {
        var allIds = VBScriptX.Grammar
                              .LiteralSymbols()
                              .Select(ls => $"{ls} - {VBScriptX.Grammar.Id(ls)}")
                              .Concat(VBScriptX.Grammar.CategorySymbols().Select(c => $"{c} - {VBScriptX.Grammar.Id(c)}")) .ToList();

        var tklFile = Path.Combine(scriptsFolder, "FullTokenList-NysaLexer.txt");
        
        if (File.Exists(tklFile))
            File.Delete(tklFile);

        using (var stream = File.OpenWrite(tklFile))
        using (var writer = new StreamWriter(stream))
        {
            foreach (var file in Directory.EnumerateFiles(scriptsFolder, "*.vbs", SearchOption.TopDirectoryOnly))
            {
                var source = File.ReadAllText(file);

                var hits = VBScriptX.Seek.Repeat(source).Where(h => h.Id != Nysa.Text.Identifier.Trivia).ToArray();

                writer.WriteLine(file);

                foreach (var hit in hits)
                {
                    var value = hit.Span.ToString();

                    if (String.IsNullOrWhiteSpace(value))
                        writer.WriteLine($"Id: {VBScriptX.Grammar.Symbol(hit.Id)} - white-space");
                    else
                        writer.WriteLine($"Id: {VBScriptX.Grammar.Symbol(hit.Id)} - {value}");
                }
            }

            writer.Flush();
            writer.Close();
        }
    }

}