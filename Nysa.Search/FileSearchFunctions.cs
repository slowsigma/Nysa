using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;

public static class FileSearchFunctions
{

    public static String RemoveBasePath(String fullPath, String basePath)
        => fullPath.Substring(basePath.Length);

    public static IEnumerable<(String FilePath, FileType FileType)> FindFiles(String baseDir, IReadOnlyDictionary<String, FileType> types)
    {
        foreach (var filePath in Directory.EnumerateFiles(baseDir, "*.*", SearchOption.AllDirectories))
        {
            var ext = Path.GetExtension(filePath);

            if (ext != null && types.ContainsKey(ext))
                yield return (FilePath: filePath, FileType: types[ext]);
        }
    }

    public static IEnumerable<SearchHit> FindHits(String filePath, FileType fileType, IEnumerable<String> findSequences)
    {
        var lines = File.ReadAllLines(filePath);

        for (Int32 l = 0; l < lines.Length; l++)
        {
            if (!fileType.ExcludeLine(lines[l]))
            {
                var start = fileType.SearchStart(lines[l]);
                var end   = fileType.SearchEnd(lines[l]);

                if (start < end)
                {
                    var line = lines[l][start..end];
                    var hit  = findSequences.Select(s => (Sequence: s, FoundAt: line.IndexOf(s)))
                                            .FirstOrNone(t =>    (t.FoundAt >= 0)
                                                              && (t.FoundAt == 0 || fileType.HitPrefixValid(line[t.FoundAt - 1]))
                                                              && (t.FoundAt + t.Sequence.Length == line.Length || fileType.HitSuffixValid(line[t.FoundAt + t.Sequence.Length])));

                    if (hit is Some<(String Sequence, Int32 FoundAt)> someFind)
                        yield return new SearchHit(someFind.Value.FoundAt, someFind.Value.Sequence.Length, l, lines[l]);
                }
            }
        }
    }

    public static void Search()
    {

    }

}