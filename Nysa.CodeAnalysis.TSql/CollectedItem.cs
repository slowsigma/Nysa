using System;

namespace Nysa.CodeAnalysis.TSql;

/// <summary>
/// CollectedItem represent the place in T-Sql where analysis found a requested item.
/// </summary>
/// <param name="SourcePosition"></param>
/// <param name="SourceLength"></param>
/// <param name="StartLine"></param>
/// <param name="StartColumn"></param>
public abstract record CollectedItem(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 StartLine,
    Int32 StartColumn
);
