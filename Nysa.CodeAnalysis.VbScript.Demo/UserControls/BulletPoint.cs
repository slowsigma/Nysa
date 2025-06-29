using System;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public record struct BulletPoint(
    String Text,
    Boolean IsImportant = false
);
