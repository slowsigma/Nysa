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
using Nysa.Logics;
using Nysa.Windows.Input;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class SemanticTreeControlViewModel : ModelObject
    {
        public SemanticTreeControlListener Listener { get; private set; }
        public ObservableCollection<SemanticNodeViewModel> Root { get; private set; }

        private RichTextBox         _SourceBox;
        private List<CodeTextPoint> _Selection;
        private VbScriptColorKey    _ColorKey;
        private CodeDocument?       _CodeDocument;
        private Int32               _LastVersion;

        public SemanticTreeControlViewModel(RichTextBox sourceBox, VbScriptColorKey colorKey)
        {
            this.Listener = new SemanticTreeControlListener();
            this.Root = new ObservableCollection<SemanticNodeViewModel>();
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

                if (this.Listener.Root is Some<Program> someProgram)
                    this.Root.Add(new SemanticNodeViewModel(this, someProgram.Value.ToCodeNodeInfo()));

                this._SourceBox.Document = this._CodeDocument.Document;

                this._LastVersion = this.Listener.Version;
            }
        }

        public void HighlightNode(SemanticNodeViewModel node)
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
