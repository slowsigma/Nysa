using System;

namespace Nysa.CodeAnalysis.TSql;

/// <summary>
/// ComplexIdentifierLookup represent the place in T-Sql where a call to OBJECT_ID() is made where the first parameter is not a literal string.
/// </summary>
/// <param name="SourcePosition"></param>
/// <param name="SourceLength"></param>
/// <param name="StartLine"></param>
/// <param name="StartColumn"></param>
public sealed record ComplexIdentifierLookup(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 StartLine,
    Int32 StartColumn
) : CollectedItem(SourcePosition, SourceLength, StartLine, StartColumn);
