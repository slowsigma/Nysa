namespace Nysa.ProjectAnalysis;

// References a .NET framework library (e.g., System.Data).
public record FrameworkReference(
    String Id
) : Reference(Id);
