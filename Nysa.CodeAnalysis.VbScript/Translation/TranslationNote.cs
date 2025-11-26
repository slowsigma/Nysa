using System;

namespace Nysa.CodeAnalysis.VbScript;

public record TranslationNote(
    String Message,
    TranslationNoteSeverity Severity
);