using System;
using System.Collections.Generic;

namespace Nysa.CodeAnalysis.TSql;

/// <summary>
/// LookupLiteral represents the first parameter in an OBJECT_ID() call when it's a literal string.
/// </summary>
/// <param name="SourcePosition"></param>
/// <param name="SourceLength"></param>
/// <param name="StartLine"></param>
/// <param name="StartColumn"></param>
/// <param name="Parts">The name parts of the identifier. Note that these SourcePosition properties of parts are relative to the start of the literal string value itself.</param>
public sealed record LookupLiteral(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 StartLine,
    Int32 StartColumn,
    IReadOnlyList<ObjectIdentifierPart> Parts
) : CollectedItem(SourcePosition, SourceLength, StartLine, StartColumn);
