using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

public static class CSharpSearch
{

    public static void FindAndDump(String baseDir, List<String> exclude_paths, String outFile, List<(String StartsWith, String ReplaceWith)> headerSwaps)
    {
        var databases   = new String[] { "Courtroom", "Financial", "FincMgmt", "Integration", "Justice", "LocalReporting", "Operations", "SessionWorks", "WorkTables" };
        var suffix_var  = new String[] { ".", "\\\"", "]." };
        var extensions  = new HashSet<String>(new String[] { ".cs", ".sql", ".resx", ".xls", ".js" }, StringComparer.OrdinalIgnoreCase);

        var variations  = databases.SelectMany(d => suffix_var.Select(s => (Title: d, Find: String.Concat(d, s)))).ToList();
        var line_end    = new Func<String, Int32>(s => s.IndexOf("//").Make(i => i < 0 ? s.Length : i));


        Boolean valid_prefix(Char character)
            =>    !Char.IsLetter(character)
            && !character.Equals('.')
            && !character.Equals('{');

        Boolean exclude_line(String line)
            =>    line.Trim().Make(t =>    t.StartsWith("[System.Xml.Serialization.XmlTypeAttribute")
                                        || t.StartsWith("[System.Xml.Serialization.XmlRootAttribute")
                                        || t.StartsWith("[Description(\"")
                                        || t.StartsWith("[MessageHandler(\"")
                                        || t.StartsWith("throw new "));

        foreach (var filePath in Directory.EnumerateFiles(baseDir, "*.*", SearchOption.AllDirectories).Where(f => !exclude_paths.Any(x => f.DataStartsWith(x)) && extensions.Contains(Path.GetExtension(f))))
        {
            var lines       = File.ReadAllLines(filePath);
            var findings    = new List<SearchHit>();

            for (Int32 l = 0; l < lines.Length; l++)
            {
                var end = line_end(lines[l]);

                if (!exclude_line(lines[l]))
                {
                    foreach (var variation in variations)
                    {
                        var trial   = lines[l].IndexOf(variation.Find, StringComparison.OrdinalIgnoreCase);

                        var hit     =    (0 <= trial)
                                    && (trial < end)
                                    && (trial < 1 || valid_prefix(lines[l][trial - 1]));

                        if (hit)
                            findings.Add(new SearchHit(trial, variation.Find.Length, l + 1, variation.Title));
                    }
                }
            }

            if (findings.Count > 0)
            {
                var header = filePath;
                
                foreach (var swap in headerSwaps)
                {
                    if (header.StartsWith(swap.StartsWith))
                        header = String.Concat(swap.ReplaceWith, header.Substring(swap.StartsWith.Length));
                }

                File.AppendAllLines(outFile, Return.Enumerable(header));

                File.AppendAllLines(outFile, findings.Select(f => String.Concat(f.LineNumber.ToString("00000"), " >>>>  ", lines[f.LineNumber - 1])));
                File.AppendAllLines(outFile, Return.Enumerable("\r\n"));
            }
        }
    }

}




