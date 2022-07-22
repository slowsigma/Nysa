using System.IO;

using Nysa.Logics;
using Nysa.ProjectAnalysis;
using Nysa.Text;

public static class VbProjectFunctions
{

    private static Suspect<IReadOnlyList<(String Name, String Value)>> ReadNameValues(this String @this)
        => Return.Try<IReadOnlyList<(String Name, String Value)>>(() =>
            File.ReadAllLines(@this)
                .Where(t => t.Contains('='))
                .Select(l => l.Split("=", StringSplitOptions.RemoveEmptyEntries))
                .Where(s => s.Length == 2)
                .Select(p => (Name: p[0], Value: p[1]))
                .ToList());

    private static Suspect<VbProject> ToProject(this IReadOnlyList<(String Name, String Value)> @this, String rootPath, String projectPath, String projectId)
    {
        var exeName = @this.FirstOrNone(p => p.Name.DataEquals("exename32")).Map(f => f.Value);
        var outPath = @this.FirstOrNone(p => p.Name.DataEquals("path32")).Map(f => f.Value).Map(o => Path.IsPathFullyQualified(o) ? o : Path.GetFullPath(o, projectPath));
        var refs    = @this.Where(p => p.Name.DataEquals("reference"))
                           .Select(f => f.Value.ToReference(rootPath, projectPath))
                           .SomeOnly()
                           .ToList();

        return (exeName is Some<String> someExe && outPath is Some<String> someOut)
               ? (new VbProject(projectId, refs, new Output(Path.GetRelativePath(rootPath, Path.Combine(someOut.Value, someExe.Value))))).Confirmed()
               : Return.Failed<VbProject>($"Cannot determine output binary: '{projectPath}'.");
    }

    private static Option<Reference> ToReference(this String @this, String rootPath, String projectPath)
    {
        var parts = @this.Split('#', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 5)
        {
            var refPath = Path.GetFullPath(parts[3], projectPath);
            var title = parts[4];
            var isTlb = Path.GetExtension(refPath).DataEquals(".tlb");

            return isTlb ? (new TypeLibReference(Path.GetRelativePath(rootPath, refPath), title.Some())).Some<Reference>()
                         : (new BinaryReference(Path.GetRelativePath(rootPath, refPath), title.Some())).Some<Reference>();
        }
        else
            return Option.None;
    }

    public static Suspect<VbProject> ReadProject(this String @this, String rootPath)
    {
        var projectId   = Path.GetRelativePath(rootPath, @this);
        var projectPath = Path.GetDirectoryName(@this);
        var nameValues  = @this.ReadNameValues();

        return (projectPath != null)
               ? nameValues.Bind(l => l.ToProject(rootPath, projectPath, projectId))
               : Return.Failed<VbProject>($"Project must be in a subfolder: '{@this}'.");
    }

}