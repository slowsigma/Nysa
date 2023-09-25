using System;

namespace Nysa.CodeAnalysis.TSql;

public record ObjectIdentifierPart(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 QuoteType,
    String Value
);
