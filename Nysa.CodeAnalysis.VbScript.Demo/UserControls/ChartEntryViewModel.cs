using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Nysa.CodeAnalysis;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class ChartEntryViewModel : ModelObject
    {
        public ChartEntry   Basis { get; private set; }
        public Int32        Index { get; private set; }
        public String       State { get; private set; }

        // private Boolean _IsCurrent;

        // public Boolean IsCurrent
        // {
        //     get { return this._IsCurrent; }
        //     set { this.UpdateValueProperty(ref this._IsCurrent, value, nameof(IsCurrent)); }
        // }


        public ChartEntryViewModel(ChartEntry basis, Int32 index)
        {
            this.Basis      = basis;
            this.Index      = index;
            this.State      = basis.ToString();
            // this._IsCurrent = false;
        }
    }

}
