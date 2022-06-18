namespace Nysa.ProjectAnalysis;

public record FrameworkProject(
    String Id,
    IReadOnlyList<Reference> References,
    IReadOnlyList<Output> Outputs           // List of outputs (this is a list as framework project may have debug and release versions)
) : Project(Id, References);
