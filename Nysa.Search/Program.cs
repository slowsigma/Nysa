using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

var baseDir     = @"C:\Projects\ODY376";
var outFile     = @"C:\Projects\ODY376\Results.txt";

var databases   = new String[] { "Courtroom", "Financial", "FincMgmt", "Integration", "Justice", "LocalReporting", "Operations", "SessionWorks", "WorkTables" };

//var swaps = new List<(String StartsWith, String ReplaceWith)>();

var exclude_paths = new List<String>();
//exclude_paths.Add(@"C:\tylerdev\mainline\repos\ody\Database\Replication");
//exclude_paths.Add(@"C:\tylerdev\mainline\repos\ody\DevInstall");

if (File.Exists(outFile))
    File.Delete(outFile);

using (var stream = File.OpenWrite(outFile))
{
    using (var writer = new StreamWriter(stream))
    {
        foreach (var pathType in FileSearchFunctions.FindFiles(baseDir, FileTypes.Index()).Where(t => !exclude_paths.Any(x => t.FilePath.DataStartsWith(x))))
        {
            var headerDone = false;

            foreach (var hit in FileSearchFunctions.FindHits(pathType.FilePath, pathType.FileType, databases))
            {
                if (!headerDone)
                {
                    writer.WriteLine(FileSearchFunctions.RemoveBasePath(pathType.FilePath, baseDir).Replace("\\", "/"));
                    headerDone = true;
                }

                writer.WriteLine(String.Concat(hit.LineNumber.ToString("00000"), " >>>>  ", hit.FullLine));
            }

            if (headerDone)
                writer.WriteLine();
        }

        writer.Flush();
        writer.Close();
    }
}



//swaps.Add((StartsWith: @"C:\Projects\ODY376\", ReplaceWith: ""));
//swaps.Add((StartsWith: @"C:\tylerdev\mainline\repos\custext", ReplaceWith: @"{Custom Extensions}"));
//swaps.Add((StartsWith: @"C:\tylerdev\mainline\repos\ody", ReplaceWith: @"{Mainline}"));


//CSharpSearch.FindAndDump(baseDir, exclude_paths, outFile, swaps);
//VisualBasicSearch.FindAndDump(baseDir, exclude_paths, outFile, swaps);