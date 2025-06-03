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

    public class ChartPositionViewModel : ModelObject
    {
        public Int32    Position    { get; private set; }
        public Token?   Token       { get; private set; }
        public String   TokenText   { get; private set; }
        public String   TokenId     { get; private set; }

        private Boolean _IsCurrent;

        public Boolean  IsCurrent
        {
            get { return this._IsCurrent; }
            set
            {
                this.UpdateValueProperty(ref this._IsCurrent, value, nameof(IsCurrent), nameof(Entries));
            }
        }

        public Int32 RuleCount => this.Entries.Count;

        public ObservableCollection<ChartEntryViewModel> Entries { get; private set; }

        public ChartPositionViewModel(Int32 position, Token? token, Grammar grammar)
        {
            this.Position  = position;
            this.Token     = token;

            var tokenText = this.Token == null
                            ? ""
                            : this.Token
                                  .Value
                                  .ToString()
                                  .Make(s => String.IsNullOrWhiteSpace(s)
                                             ? grammar.Symbol(this.Token.Value.Id.Values().First())
                                             : s); 

            this.TokenText = tokenText;
            this.TokenId   = this.Token?.Id.ToString() ?? String.Empty;
            this.Entries   = new ObservableCollection<ChartEntryViewModel>();

            this._IsCurrent = false;
        }

        public void Update(BasicChart chart)
        {
            this.Entries.Clear();

            var focus = chart.Entries(this.Position);

            for (Int32 i = focus.Count - 1; i >= 0; i--)
                this.Entries.Add(new ChartEntryViewModel(focus[i], i));

            this.NotifyChanged(nameof(RuleCount));
        }
    }

}
