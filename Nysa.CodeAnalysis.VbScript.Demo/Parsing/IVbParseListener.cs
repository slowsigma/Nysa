using System;
using System.Threading;

using Nysa.CodeAnalysis.VbScript;
using Nysa.Logics;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public interface IVbParseListener : IParseListener, IDisposable
{

    Boolean InProgress { get; }
    Boolean IsCancelled { get; }
    (Int32 CurrentChartIndex, Int32 CurrentEntryIndex, ParseChart Chart, Int32 ChangedChartIndex)? CurrentState();
    InverseChart? Inverse();
    void Continue(Boolean fastForward = false);
    void Cancel();
}
