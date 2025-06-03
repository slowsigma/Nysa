using System;
using Nysa.ComponentModel;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class MistakeControlViewModel : ModelObject
{

    private Int32  _AverageSalaryInThousands;
    private Double _EmployeeCostFactor;
    private Int32  _TotalEmloyees;
    private Int32  _TotalYears;
    private Double _FrontEndFactor;

    public String AverageSalaryInThousands
    {
        get { return this._AverageSalaryInThousands.ToString(); }
        set
        {
            this.UpdateValueProperty(ref this._AverageSalaryInThousands,
                                     value.ParseInt32().GetValueOrDefault(this._AverageSalaryInThousands),
                                     nameof(AverageSalaryInThousands),
                                     nameof(SingleEmployeeCost),
                                     nameof(TotalEmployeeCostPerYear),
                                     nameof(TotalProductCost),
                                     nameof(TotalProductFontEndCost));
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

    
    public MistakeControlViewModel()
    {
        this._AverageSalaryInThousands  = 72;
        this._EmployeeCostFactor        = 1.4;
        this._TotalEmloyees             = 100;
        this._TotalYears                = 20;
        this._FrontEndFactor            = 0.5;
    }

}