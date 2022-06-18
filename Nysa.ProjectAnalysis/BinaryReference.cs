namespace Nysa.ProjectAnalysis;

using Nysa.Logics;

public record BinaryReference(
    String Id,
    Option<String> Title
) : Reference(Id);
