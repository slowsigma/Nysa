using System.IO;

using Nysa.Logics;
using Nysa.ProjectAnalysis;
using Nysa.Text;

if (args.Length < 1)
    return;

if (!Directory.Exists(args[0]))
{
    Console.WriteLine("Project analysis requires a top level directory argument.");
    return;
}

var basePath     = args[0];
var projectExts  = new HashSet<String>(Return.Enumerable(".vbp", ".csproj"), StringComparer.OrdinalIgnoreCase);
var projectFiles = new List<String>();

foreach (var file in Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories))
{
    if (projectExts.Contains(Path.GetExtension(file)))
        projectFiles.Add(Path.GetRelativePath(basePath, file));

    if (Path.GetExtension(file).DataEquals(".vbp"))
    {
        var projectPath = Path.GetDirectoryName(file);
        var lines       = file.ReadNameValues();

        if (projectPath != null)
        {
            var vbProj = lines.Map(l => l.ToProject(basePath, projectPath, Path.GetRelativePath(basePath, file)));
        }
        
    }
}

Console.WriteLine("Project files found.");