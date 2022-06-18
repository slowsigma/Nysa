namespace Nysa.ProjectAnalysis;

using Nysa.Logics;

public record TypeLibReference(
    String Id,
    Option<String> Title
) : Reference(Id);
