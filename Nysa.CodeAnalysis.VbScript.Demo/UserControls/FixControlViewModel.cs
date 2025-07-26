using System;
using Nysa.ComponentModel;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class FixControlViewModel : ModelObject
{
    private static readonly Int32 WeeksPerYear = 52;
    private static readonly Int32 BusinessDaysPerWeek = 5;
    private static readonly Int32 MaxDaysPerWeek = 7;
    private static readonly Int32 MaxDaysPerYear = WeeksPerYear * MaxDaysPerWeek;


    private Int32 _TotalLinesOfCode;
    private Int32 _LinesOfCodePerDay;       // per developer: standard average is between 10 and 40
    private Int32 _TotalDevelopers;
    private Int32 _WorkDaysPerYear;         // standard full time is 260 (52 weeks * 5 days)
    private Int32 _BugsPerThousandLines;    // standard average is between 1 and 25

    private Int32 DaysToRecodeNumber => this._TotalLinesOfCode / (this._LinesOfCodePerDay * this._TotalDevelopers);
    private Double YearsToRecodeNumber => Convert.ToDouble(this.DaysToRecodeNumber) / Convert.ToDouble(this._WorkDaysPerYear);
    private Int32 BugsIntroducedNumber => (this._TotalLinesOfCode / 1000) * this._BugsPerThousandLines;

    public String DaysToRecode => this.DaysToRecodeNumber.ToString("#,##0");
    public String YearsToRecode => this.YearsToRecodeNumber.ToString("#,##0.00");
    public String BugsIntroduced => this.BugsIntroducedNumber.ToString("#,##0");

    public String TotalLinesOfCode
    {
        get { return this._TotalLinesOfCode.ToString("#,##0"); }
        set
        {
            this.UpdateValueProperty(ref this._TotalLinesOfCode,
                                     value.Replace(",", "").ParseInt32().GetValueOrDefault(this._TotalLinesOfCode),
                                     nameof(TotalLinesOfCode),
                                     nameof(DaysToRecode),
                                     nameof(YearsToRecode),
                                     nameof(BugsIntroduced));
        }
    }

    public String LinesOfCodePerDay
    {
        get { return this._LinesOfCodePerDay.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._LinesOfCodePerDay,
                                     value.ParseInt32().GetValueOrDefault(this._LinesOfCodePerDay),
                                     nameof(LinesOfCodePerDay),
                                     nameof(DaysToRecode),
                                     nameof(YearsToRecode));
        }
    }

    public String TotalDevelopers
    {
        get { return this._TotalDevelopers.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._TotalDevelopers,
                                     value.ParseInt32().GetValueOrDefault(this._TotalDevelopers),
                                     nameof(TotalDevelopers),
                                     nameof(DaysToRecode),
                                     nameof(YearsToRecode));
        }
    }

    public String WorkDaysPerYear
    {
        get { return this._WorkDaysPerYear.ToString(); }
        set
        {
            if (value.ParseInt32().GetValueOrDefault(this._WorkDaysPerYear).Make(v => v < 1 || v > MaxDaysPerYear))
            {
                value = this._WorkDaysPerYear.ToString();
            }

            this.UpdateValueProperty(ref this._WorkDaysPerYear,
                                     value.ParseInt32().GetValueOrDefault(this._WorkDaysPerYear),
                                     nameof(WorkDaysPerYear),
                                     nameof(YearsToRecode));
        }
    }

    public String BugsPerThousandLines
    {
        get { return this._BugsPerThousandLines.ToString(); }
        set
        {
            if (value.ParseInt32().GetValueOrDefault(this._BugsPerThousandLines).Make(v => v < 1 || v > 25))
            {
                value = this._BugsPerThousandLines.ToString();
            }

            this.UpdateValueProperty(ref this._BugsPerThousandLines,
                                     value.ParseInt32().GetValueOrDefault(this._BugsPerThousandLines),
                                     nameof(BugsPerThousandLines),
                                     nameof(BugsIntroduced));
        }
    }

    
    public FixControlViewModel()
    {
        this._TotalLinesOfCode          = 2100000;
        this._LinesOfCodePerDay         = 40;           // standard: 10 .. 40
        this._TotalDevelopers           = 100;
        this._WorkDaysPerYear           = WeeksPerYear * BusinessDaysPerWeek; // 260
        this._BugsPerThousandLines      = 5;            // standard:  1 .. 25
    }

}