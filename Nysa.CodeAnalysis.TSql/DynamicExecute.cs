using System;

namespace Nysa.CodeAnalysis.TSql;

/// <summary>
/// DynamicExecute represents an instance of an EXEC()/EXECUTE() where a literal string or string variable is being executed.
/// </summary>
/// <param name="SourcePosition"></param>
/// <param name="SourceLength"></param>
/// <param name="StartLine"></param>
/// <param name="StartColumn"></param>
public sealed record DynamicExecute(
    Int32 SourcePosition,
    Int32 SourceLength,
    Int32 StartLine,
    Int32 StartColumn
) : CollectedItem(SourcePosition, SourceLength, StartLine, StartColumn);
