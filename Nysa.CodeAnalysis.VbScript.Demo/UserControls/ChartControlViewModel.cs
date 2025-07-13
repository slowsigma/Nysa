// THIS IS GOING TO ESSENTIALLY BE THE SAME AS ParseTestingViewModel.cs BUT WILL FOCUS
// ON ALLOWING SOURCE TO CHANGE AS WELL AS FOR CONTROLLING AND RESETTING THE PARSING
// PROCESS THROUGH VbParseListener.cs



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class ChartControlViewModel : ModelObject
    {
        public ChartControlListener Listener { get; private set; }

        private IVbParseListener?   _ParseListener;
        private Int32               _ParseState;
        private Thread?             _ParsingThread;
        private DispatcherTimer     _PlayTimer;
        private Int32               _LastVersion;


        public ChartViewModel Chart { get; private set; }

        public Boolean CanParse => this.Chart.Positions.Count > 0;
        public Visibility PauseVisibility => this._PlayTimer.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PlayVisibility => this._PlayTimer.IsEnabled ? Visibility.Collapsed : Visibility.Visible;

        private Boolean InverseReady() => this._ParseListener.Make(l => l != null && !l.IsCancelled && l.Inverse != null);

        public Boolean IsInverseOptionEnabled => this.InverseReady();

        private Boolean _ShowInverse;

        public Boolean ShowInverse
        {
            get { return this._ShowInverse; }
            set
            {
                if (this.UpdateValueProperty(ref this._ShowInverse, value, nameof(ShowInverse)))
                {
                    this.Chart.Reset(this.Listener.Tokens);

                    if (this._ParseListener != null)
                    {
                        var invserse = this._ParseListener.Inverse();
                        var state = this._ParseListener.CurrentState();

                        if (value && (invserse != null))
                        {
                            var lastIndex = invserse.Data.Count - 1;

                            this.Chart.UpdateState(invserse.Data.Count - 1, 0, invserse, lastIndex);
                        }
                        else if (state != null)
                        {
                            this.Chart.UpdateState(state.Value.CurrentChartIndex, state.Value.CurrentEntryIndex, state.Value.Chart, state.Value.ChangedChartIndex);
                        }
                    }
                }
            }
        }

        public ICommand RewindCommand { get; private set; }
        public ICommand StepCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand InvertCommand { get; private set; }

        public ChartControlViewModel()
        {
            this.Listener = new ChartControlListener();
            this.Chart = new ChartViewModel(VbScript.Language.Grammar);
            this._ParseState = 0;
            this._ParsingThread = null;
            this._ParseListener = null;
            this._ShowInverse = false;

            this.RewindCommand = new Command(this.Rewind, null);
            this.StepCommand = new Command(this.SingleStep, null);
            this.PlayCommand = new Command(this.PlayOrPause, null);
            this.PauseCommand = new Command(this.PlayOrPause, null);
            this.ForwardCommand = new Command(this.Forward, null);
            this.StopCommand = new Command(this.Cancel, null);

            this._PlayTimer = new DispatcherTimer();
            this._PlayTimer.Interval = new TimeSpan(0, 0, 0, 0, 120);
            this._PlayTimer.Tick += (a, b) => { this.SingleStep(); };

            this._LastVersion = -1;
        }

        private IVbParseListener CreateParseListener() => new VbParseListener();

        public void OnEnter()
        {
            if (this.Listener.IsBackgroundComplete() && this.Listener.Version != this._LastVersion)
            {
                this.Chart.Reset(this.Listener.Tokens);

                this._LastVersion = this.Listener.Version;
            }

            this.NotifyChanged(nameof(CanParse));
        }

        public void OnLeave()
        {
            if (this._ParseListener != null && !this._ParseListener.IsCancelled)
                this._ParseListener.Cancel();

            if (this._PlayTimer.IsEnabled)
                this._PlayTimer.Stop();

            if (this._ParsingThread != null)
                this._ParsingThread.Join();

            this._ParsingThread = null;

            if (this._ParseListener != null)
            {
                this._ParseListener.Dispose();
                this._ParseListener = null;
            }


            this._ParseState = 0;

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility), nameof(IsInverseOptionEnabled));
        }

        private void ParseOperation()
        {
            if (this._ParseListener == null)
                return;

            this._ParseState = 1;

            var parse = Language.Parse(this.Listener.Code, this._ParseListener);

            this._ParseState = -1;
        }

        private void Rewind()
        {
            this.Cancel();

            this.Chart.Reset(this.Listener.Tokens);

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility), nameof(IsInverseOptionEnabled));
        }

        private void SingleStep()
        {
            if (this._ParsingThread == null)
            {
                this._ParseListener = this.CreateParseListener();
                this._ParsingThread = new Thread(ParseOperation);
                this._ParsingThread.Start();

                while (this._ParseListener.InProgress)
                    Thread.Sleep(60);

                var state = this._ParseListener.CurrentState();

                if (state != null)
                    this.Chart.UpdateState(state.Value.CurrentChartIndex, state.Value.CurrentEntryIndex, state.Value.Chart, state.Value.ChangedChartIndex);
            }
            else if (this._ParseListener != null && this._ParseState > 0)
            {
                this._ParseListener.Continue();

                while (this._ParseListener.InProgress && this._ParseState > 0)
                    Thread.Sleep(60);

                var state = this._ParseListener.CurrentState();

                if (state != null)
                    this.Chart.UpdateState(state.Value.CurrentChartIndex, state.Value.CurrentEntryIndex, state.Value.Chart, state.Value.ChangedChartIndex);
            }

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility), nameof(IsInverseOptionEnabled));
        }

        private void PlayOrPause()
        {
            if (this._PlayTimer.IsEnabled)
                this._PlayTimer.Stop();
            else
                this._PlayTimer.Start();

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility));
        }

        private void Forward()
        {
            if (this._ParsingThread == null)
            {
                this._ParseListener = this.CreateParseListener();
                this._ParsingThread = new Thread(ParseOperation);
                this._ParsingThread.Start();

                while (this._ParseListener.InProgress)
                    Thread.Sleep(60);
            }

            if (this._ParseListener != null && this._ParseState > 0)
            {
                this._ParseListener.Continue(true);

                while (this._ParseListener.InProgress && this._ParseState > 0)
                    Thread.Sleep(60);
            }

            var state = this._ParseListener.CurrentState();

            if (state != null)
                this.Chart.UpdateState(state.Value.CurrentChartIndex, state.Value.CurrentEntryIndex, state.Value.Chart, state.Value.ChangedChartIndex);

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility), nameof(IsInverseOptionEnabled));
        }

        private void Cancel()
        {
            if (this._PlayTimer.IsEnabled)
                this._PlayTimer.Stop();

            if (this._ParseListener != null)
                this._ParseListener.Cancel();

            if (this._ParsingThread != null)
                this._ParsingThread.Join();

            this._ParseState = 0;
            this._ParsingThread = null;

            this.NotifyChanged(nameof(PlayVisibility), nameof(PauseVisibility), nameof(IsInverseOptionEnabled));
        }

    }

}