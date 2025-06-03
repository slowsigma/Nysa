using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nysa.CodeAnalysis;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class ChartViewModel : ModelObject
    {
        private Grammar _Grammar;

        public ObservableCollection<ChartPositionViewModel> Positions { get; private set; }

        private ChartPositionViewModel? _CurrentPosition;

        public ChartPositionViewModel? CurrentPosition
        {
            get { return this._CurrentPosition; }
            set { this.UpdateObjectProperty(ref this._CurrentPosition, value, nameof(CurrentPosition)); }
        }

        private Int32 _LastChartIndex;
        private Int32 _LastEntryIndex;

        public ChartViewModel(Grammar grammar)
        {
            this._Grammar  = grammar;
            this.Positions = new ObservableCollection<ChartPositionViewModel>();

            this._LastChartIndex = -1;
            this._LastEntryIndex = 0;

            this._CurrentPosition = this.Positions.Count > 0 ? this.Positions[0] : null;
        }

        public void Reset(Token[] tokens)
        {
            this.Positions.Clear();

            for (Int32 i = 0; i < (tokens.Length + 1); i++)
            {
                var current = (i < tokens.Length) ? (Token?)tokens[i] : null;

                this.Positions.Add(new ChartPositionViewModel(i, current, this._Grammar));
            }

            this._LastChartIndex = 0;
            this._LastEntryIndex = 0;

            this.CurrentPosition = this.Positions[0];
        }

        public void UpdateState(Int32 currentChartIndex, Int32 currentEntryIndex, BasicChart chart, Int32 changedChartIndex)
        {
            while (this._LastChartIndex < currentChartIndex)
            {
                if (this._LastChartIndex > -1)
                {
                    this.Positions[this._LastChartIndex].Update(chart);
                    this.Positions[this._LastChartIndex].IsCurrent = false;
                }
                this._LastChartIndex++;
            }

            this.Positions[this._LastChartIndex].Update(chart);
            this.Positions[this._LastChartIndex].IsCurrent = true;
            
            this._LastEntryIndex = currentEntryIndex;

            this.CurrentPosition = this.Positions[this._LastChartIndex];
        }
    }

}
