using System;
using System.Collections.Generic;

namespace Nysa.CodeAnalysis.TSql;

public sealed record ObjectIdentifier(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 StartLine,
    Int32 StartColumn,
    Boolean CanAlias,
    Boolean IsSelectInto,
    IReadOnlyList<ObjectIdentifierPart> Parts
) : CollectedItem(SourcePosition, SourceLength, StartLine, StartColumn);
