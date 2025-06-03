using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class TokensControlViewModel : ModelObject, IHighlight<TokenViewModel>
    {
        public TokensControlListener Listener { get; private set; }

        public ObservableCollection<TokenViewModel> Items { get; private set; }

        private RichTextBox      _SourceBox;
        private CodeTextPoint?   _Selection;
        private VbScriptColorKey _ColorKey;
        private CodeDocument?    _CodeDocument;
        private Int32            _LastVersion;

        public TokensControlViewModel(RichTextBox sourceBox, VbScriptColorKey colorKey)
        {
            this.Listener = new TokensControlListener();
            this._SourceBox = sourceBox;
            this.Items = new ObservableCollection<TokenViewModel>();
            this._ColorKey = colorKey;
            this._CodeDocument = null;
            this._LastVersion = -1;
        }

        public void OnEnter()
        {
            if (this.Listener.IsBackgroundComplete() && this.Listener.Version != this._LastVersion)
            {
                this._CodeDocument = CodeDocument.Create(this.Listener.Code, this.Listener.Tokens, this._ColorKey, 14.0, VbScriptTextFunctions.CommentLines(this.Listener.Code), VbScriptColorKey.Green);
                this._CodeDocument.ApplyColors(VbScriptColorKey.Index);

                this._SourceBox.Document = this._CodeDocument.Document;

                this.Items.Clear();

                foreach (var token in this.Listener.Tokens)
                    this.Items.Add(new TokenViewModel(this, token));

                this._LastVersion = this.Listener.Version;
            }
        }

        private void HighlightNode(TokenViewModel tokenViewModel)
        {
            if (this._CodeDocument == null)
                return;
                
            if (this._Selection != null)
            {
                this._Selection.Value.Range.ClearAllProperties();
                this._Selection.Value.Range.ApplyPropertyValue(TextElement.ForegroundProperty, VbScriptColorKey.Index[this._Selection.Value.ColorNumber]);
            }

            this._Selection = this._CodeDocument.TextPoints.FirstOrDefault(p => p.Token != null && p.Token.Value.Span.Position == tokenViewModel.Basis.Span.Position);

            if (this._Selection != null)
            {
                this._Selection.Value.Range.ClearAllProperties();
                this._Selection.Value.Range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
            }
        }

        public void Highlight(TokenViewModel item)
        {
            this.HighlightNode(item);
        }
    }

}
