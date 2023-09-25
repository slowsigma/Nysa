using System;
using System.Collections.Generic;

namespace Nysa.CodeAnalysis.TSql;

public record TSqlAnalysis<T>(
    T Content,
    IReadOnlyList<CollectedItem> CollectedItems
);
