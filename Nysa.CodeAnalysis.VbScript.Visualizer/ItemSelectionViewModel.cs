﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using Nysa.CodeAnalysis.VbScript;
using Nysa.CodeAnalysis.VbScript.Semantics;

using Nysa.Logics;
using Nysa.Windows.Input;

using VBScript = Nysa.CodeAnalysis.VbScript.Language;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public class ItemSelectionViewModel : NormalWindowViewModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICollectionView _AvailableScriptItemsView;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ObservableCollection<Content> _AvailableScriptItems;
        public ObservableCollection<Content> ScriptItems { get { return this._AvailableScriptItems; } }

        public String _Folder;
        private Content? _SelectedScriptItem;

        public Content? SelectedScriptItem
        {
            get { return this._SelectedScriptItem; }
            set
            {
                if (this.UpdateObjectProperty(ref this._SelectedScriptItem, value, nameof(SelectedScriptItem)))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String? _CodeFilterString;
        public String? CodeFilterString
        {
            get { return this._CodeFilterString; }
            set
            {
                if (this.UpdateObjectProperty(ref this._CodeFilterString, value, nameof(CodeFilterString)))
                {
                    this.NotifyChanged(nameof(ScriptItems));
                    this._AvailableScriptItemsView.Refresh();

                    if (this._AvailableScriptItemsView.Cast<Object>().Count() == 1)
                        this.SelectedScriptItem = this._AvailableScriptItemsView.Cast<Content>().First();
                    else
                        this.SelectedScriptItem = null;
                }
            }
        }

        public new ItemSelectionView Window => (ItemSelectionView)base.Window;

        public ItemSelectionViewModel(String folder, ItemSelectionView window)
            : base(window)
        {
            this._Folder = folder;
            this._AvailableScriptItems = new ObservableCollection<Content>();
            this._AvailableScriptItemsView = CollectionViewSource.GetDefaultView(this._AvailableScriptItems);
            this._AvailableScriptItemsView.Filter = this.ScriptItemFilter;

            this._SelectedScriptItem = null;

            this.Window.Loaded += Window_Loaded;
            this.Window._ShowLex.Click += _ShowLex_Click;
            this.Window._ShowParse.Click += _ShowParse_Click;
        }

        private void _ShowLex_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedScriptItem is VbScriptContent content)
            {
                var tokens = VBScript.Lex(content.Value);

                var view = (new LexVisualizer()).Bound(vw => new LexVisualizerViewModel(vw, content.Source, content.Value, tokens)).View;
                view.Show();
            }
        }

        private void _ShowParse_Click(object sender, RoutedEventArgs e)
        {
            var views = new List<ParseVisualizer>();

            if (this.SelectedScriptItem is Content content)
            {
                var parse = content.Parse();

                if (parse is Confirmed<Parse> goodParse)
                {
                    if (goodParse.Value is HtmlParse html)
                        html.VbsParses.Affect(p => views.Add((new ParseVisualizer()).Bound(vw => new ParseVisualizerViewModel(vw, html.Content.Source, p.Content.Value, p.SemanticRoot)).View));
                    else if (goodParse.Value is VbScriptParse vbs)
                        views.Add((new ParseVisualizer()).Bound(vw => new ParseVisualizerViewModel(vw, content.Source, content.Value, vbs.SemanticRoot)).View);
                }
            }

            foreach (var view in views)
                view.Show();
        }

        private async void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var sources = await ParseItemsLoader.GetSourcesAsync(this._Folder);

            sources.Affect(s => { if (s is Confirmed<Content> c) this._AvailableScriptItems.Add(c.Value); });

            this.NotifyChanged(nameof(ScriptItems));
        }

        private static Boolean MatchAll(String filterString, String testValue)
        {
            var filters = filterString.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return filters.All(f => testValue.IndexOf(f, 0, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private Boolean ScriptItemFilter(object item)
        {
            return (item as Content != null)
                   ? String.IsNullOrWhiteSpace(this._CodeFilterString) || MatchAll(this._CodeFilterString ?? String.Empty, ((Content)item).Source)
                   : false;
        }



    }

}
