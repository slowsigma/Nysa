using System;

namespace Nysa.CodeAnalysis.TSql;

public record Synonym(
    String Database,
    String Schema,
    String Name,
    String ReferencesDatabase,
    String ReferencesSchema,
    String ReferencesOject
);
