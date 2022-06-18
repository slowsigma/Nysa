namespace Nysa.ProjectAnalysis;

public record VbProject(
    String Id,
    IReadOnlyList<Reference> References,
    Output Output
) : Project(Id, References);
