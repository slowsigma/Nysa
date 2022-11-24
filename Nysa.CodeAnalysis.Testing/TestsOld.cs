using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text.Parsing;
using Nysa.Text.Lexing;


namespace Nysa.CodeAnalysis.Testing;

using Dorata.Text.Lexing;
//using Dorata.Text.Parsing;
using DorataVBScript = Dorata.Text.Parsing.VBScript;

public static class TestsOld
{

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

}