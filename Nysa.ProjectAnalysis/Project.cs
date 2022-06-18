namespace Nysa.ProjectAnalysis;

public abstract record Project(
    String Id,                              // Relative file path of a project file
    IReadOnlyList<Reference> References     // List of Reference objects depending on the type of project
);
