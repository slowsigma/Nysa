using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.CodeAnalysis.VbScript;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;
using Nysa.Text.Parsing;
using Nysa.Windows.Input;

using Nysa.CodeAnalysis.Documents;
using Nysa.ComponentModel;
using System.Windows.Controls;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class ParseTreeControlViewModel : ModelObject
    {
        public ParseTreeControlListener Listener { get; private set; }
        public ObservableCollection<ParseNodeViewModel> Root { get; private set; }

        private RichTextBox         _SourceBox;
        private List<CodeTextPoint> _Selection;
        private VbScriptColorKey    _ColorKey;
        private CodeDocument?       _CodeDocument;
        private Int32               _LastVersion;


        public ParseTreeControlViewModel(RichTextBox sourceBox, VbScriptColorKey colorKey)
        {
            this.Listener = new ParseTreeControlListener();
            this.Root = new ObservableCollection<ParseNodeViewModel>();
            this._SourceBox = sourceBox;
            this._Selection = new List<CodeTextPoint>();
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
                this.Root.Clear();
                this._Selection.Clear();

                if (this.Listener.Root is Some<Node> someRoot)
                    this.Root.Add(new ParseNodeViewModel(this, (NodeOrToken)someRoot.Value));

                this._SourceBox.Document = this._CodeDocument.Document;

                this._LastVersion = this.Listener.Version;
            }
        }
        

        public void HighlightNode(ParseNodeViewModel node)
        {
            if (this._CodeDocument == null)
                return;
                
            if (this._Selection.Count > 0)
            {
                foreach (var point in this._Selection)
                {
                    point.Range.ClearAllProperties();
                    point.Range.ApplyPropertyValue(TextElement.ForegroundProperty, VbScriptColorKey.Index[point.ColorNumber]);
                }
            }

            this._Selection.Clear();

            if (node == this.Root[0])
                return; // do not highlight all the code

            var position = node.Position;
            var length = node.Length;

            if (position is Some<Int32> somePos && length is Some<Int32> someLen)
            {
                this._Selection.AddRange(this._CodeDocument.TextPoints.Where(p => p.Token != null && p.Token.Value.Span.Position >= somePos.Value && p.Token.Value.Span.Position <= somePos.Value + someLen.Value));
            }

            if (this._Selection.Count > 0)
            {
                foreach (var point in this._Selection)
                {
                    point.Range.ClearAllProperties();
                    point.Range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
                }
            }
        }

    }

}
