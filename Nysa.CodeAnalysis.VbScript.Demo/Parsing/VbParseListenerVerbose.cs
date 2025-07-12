using System;
using System.Threading;

using Nysa.CodeAnalysis.VbScript;
using Nysa.Logics;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

/*

Notes on how this will work:
1. We create a separate thread for parsing some VbScript.
2. We give an instance of this class to that thread.
3. That thread will immediately create the first change
   and stop in the ChartChanged method waiting on _Continue.
4. We build a display to hold show the chart as it changes.
   The display has a continue button and a stop button.
5. The display can perform thread sleep if no state is present.
6. When the listener is Ready, the display will refresh.
7. The display continue button can perform the necessary steps to
   call listener continue, wait for Ready, and refresh display.
8. The stop button will call the listener cancel and maybe clear the display.

*/

public class VbParseListenerVerbose : IVbParseListener
{
    private ManualResetEvent    _Continue;
    private Boolean             _Cancel;

    // these come from Nysa.CodeAnalsysis
    private Int32               _CurrentChartIndex;
    private Int32               _CurrentEntryIndex;
    private ParseChart?         _Chart;
    private Int32               _ChangedChartIndex;
    private InverseChart?       _InverseChart;
    private Boolean             _NoWait;

    public VbParseListenerVerbose()
    {
        this._Continue = new ManualResetEvent(true);
        this._Cancel = false;
        this._CurrentChartIndex = -1;
        this._Chart = null;
        this._InverseChart = null;
        this._NoWait = false;
    }

    public Boolean InProgress => this._Continue.WaitOne(0);
    

    public Boolean IsCancelled => this._Cancel;

    public (Int32 CurrentChartIndex, Int32 CurrentEntryIndex, ParseChart Chart, Int32 ChangedChartIndex)? CurrentState()
    {
        if (this._Chart != null)
            return (this._CurrentChartIndex, this._CurrentEntryIndex, this._Chart, this._ChangedChartIndex);
        else
            return null;
    }

    public InverseChart? Inverse()
    {
        return this._InverseChart;
    }

    public void Continue(Boolean fastForward = false)
    {
        this._NoWait = fastForward;
        this._Continue.Set();
    }

    public void Cancel()
    {
        this._Cancel = true;
        this._Continue.Set();
    }

    void ICreateChartListener.ChartChanged(int changeIndex, ParseChart chart)
    {
        // this overload is obsolete
    }

    void ICreateChartListener.ChartChanged(int currentChartIndex, int currentEntryIndex, ParseChart chart, int changeChartIndex)
    {
        if (this._NoWait)
            return;
            
        if (!this._Cancel && (this._CurrentChartIndex != currentChartIndex || this._CurrentEntryIndex != currentChartIndex))
        {
            this._CurrentChartIndex = currentChartIndex;
            this._CurrentEntryIndex = currentEntryIndex;
            this._Chart = chart;
            this._ChangedChartIndex = changeChartIndex;

            if (!this._Cancel)
                this._Continue.Reset();

            if (!this._Cancel)
                this._Continue.WaitOne();
        }
    }

    void IParseListener.ChartInverted(Grammar grammar, InverseChart chart)
    {
        if (!this._Cancel)
            this._InverseChart = chart;
    }

    void IParseListener.CreateChartEnded(Grammar grammar, ParseChart chart)
    {
        if (!this._Cancel)
        {
            this._Chart = chart;
            this._CurrentChartIndex = chart.Data.Count - 1;
            this._CurrentEntryIndex = 0;
            this._ChangedChartIndex = this._CurrentChartIndex;
        }
    }

    public void Dispose()
    {
        this._Cancel = true;
        this._Continue.Set();
        this._Continue.Dispose();
    }

}
