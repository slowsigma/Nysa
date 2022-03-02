using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.CodeAnalysis.VbScript;
using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public static class ParseItemsLoader
    {
        // private static readonly String SCRIPT_SELECT    = "SELECT [ScriptID], CONVERT(VARCHAR(MAX), [ScriptCode]) FROM [Operations].[dbo].[sScript] WHERE NOT [ScriptCode] IS NULL;";
        // private static readonly String PRIMARY_SQL      = @"Data Source=PLADVSVNEO\NEO;Initial Catalog=Operations;Integrated Security=True;Application Name=CodeVisualizer";
        private static readonly String[] EXTENSIONS     = { "*.vbs", "*.htm" };

        public static async Task<List<Suspect<Content>>> GetSourcesAsync(String folder)
        {
            var done = await Task.Run(() =>
            {
                var files   = Return.Enumerable(folder)
                                    .SelectMany(w => EXTENSIONS.Select(e => (Web: w, Ext: e)))
                                    .SelectMany(x => Directory.EnumerateFiles(x.Web, x.Ext, SearchOption.AllDirectories))
                                    .Select(f => f.ToContent());

                return files.ToList();
            });

            return done;
        }
    }

}
