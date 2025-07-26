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
    private Int32 _LinesOfCodePerDay;       // per developer
    private Int32 _NumberOfFullTimeDevs;
    private Int32 _WorkDaysPerYear;
    private Int32 _BugsPerThousandLines;

    public Int32 DaysToRecodeNumber => this._TotalLinesOfCode / (this._LinesOfCodePerDay * this._NumberOfFullTimeDevs);
    public Double YearsToRecodeNumber => Convert.ToDouble(this.DaysToRecodeNumber) / Convert.ToDouble(this._WorkDaysPerYear);
    public Int32 TotalBugsIntroducedNumber => this._TotalLinesOfCode / this._BugsPerThousandLines;

    public String DaysToRecode => this.DaysToRecodeNumber.ToString("#,##0");
    public String YearsToRecode => this.YearsToRecodeNumber.ToString("#,##0.00");
    public String BugsIntroduced => this.TotalBugsIntroducedNumber.ToString("#,##0");

    public String TotalLinesOfCode
    {
        get { return this._TotalLinesOfCode.ToString("#,##0"); }
        set
        {
            this.UpdateValueProperty(ref this._TotalLinesOfCode,
                                     value.Replace(",", "").ParseInt32().GetValueOrDefault(this._TotalLinesOfCode),
                                     nameof(TotalLinesOfCode));
        }
    }


    public String EmployeeCostFactor
    {
        get { return this._EmployeeCostFactor.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._EmployeeCostFactor,
                                     value.ParseDouble().GetValueOrDefault(this._EmployeeCostFactor),
                                     nameof(EmployeeCostFactor),
                                     nameof(SingleEmployeeCost),
                                     nameof(TotalEmployeeCostPerYear),
                                     nameof(TotalProductCost),
                                     nameof(TotalProductFontEndCost));
        }
    }

    public String TotalEmployees
    {
        get { return this._TotalEmloyees.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._TotalEmloyees,
                                     value.ParseInt32().GetValueOrDefault(this._TotalEmloyees),
                                     nameof(TotalEmployees),
                                     nameof(TotalEmployeeCostPerYear),
                                     nameof(TotalProductCost),
                                     nameof(TotalProductFontEndCost));
        }
    }

    public String TotalYears
    {
        get { return this._TotalYears.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._TotalYears,
                                     value.ParseInt32().GetValueOrDefault(this._TotalYears),
                                     nameof(TotalYears),
                                     nameof(TotalProductCost),
                                     nameof(TotalProductFontEndCost));
        }
    }

    public String FrontEndFactor
    {
        get { return this._FrontEndFactor.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._FrontEndFactor,
                                     value.ParseDouble().GetValueOrDefault(this._FrontEndFactor),
                                     nameof(FrontEndFactor),
                                     nameof(TotalProductFontEndCost));
        }
    }

    private Int32 SingleEmployeeCostNumber       => Convert.ToInt32((this._AverageSalaryInThousands * 1000.0 * this._EmployeeCostFactor));
    private Int32 TotalEmployeeCostPerYearNumber => (this.SingleEmployeeCostNumber * this._TotalEmloyees);
    private Int32 TotalProductCostNumber         => (this.TotalEmployeeCostPerYearNumber * this._TotalYears);
    private Int32 TotalProductFrontEndCostNumber => Convert.ToInt32((this.TotalProductCostNumber * this._FrontEndFactor));

    public String SingleEmployeeCost             => this.SingleEmployeeCostNumber.ToString("c");
    public String TotalEmployeeCostPerYear       => this.TotalEmployeeCostPerYearNumber.ToString("c");
    public String TotalProductCost               => this.TotalProductCostNumber.ToString("c");
    public String TotalProductFontEndCost        => this.TotalProductFrontEndCostNumber.ToString("c");

    
    public FixControlViewModel()
    {
        this._TotalLinesOfCode          = 2100000;
        this._LinesOfCodePerDay         = 40;           // standard: 10 .. 40
        this._NumberOfFullTimeDevs      = 100;
        this._WorkDaysPerYear           = WeeksPerYear * BusinessDaysPerWeek; // 260
        this._BugsPerThousandLines      = 10;           // standard:  1 .. 25
    }

}