using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript;
using Nysa.CodeAnalysis.VbScript.Rescript;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class RescriptControlViewModel : ModelObject
    {
        public RescriptControlListener Listener { get; private set; }

        private RichTextBox         _SourceBox;
        private RichTextBox         _SourceBoxRescript;
        private VbScriptColorKey    _ColorKey;
        private CodeDocument?       _CodeDocument;
        private CodeDocument?       _CodeDocumentRescript;
        private Int32               _LastVersion;

        public ICommand RewindCommand { get; private set; }
        public ICommand RescriptCommand { get; private set; }

        public RescriptControlViewModel(RichTextBox sourceBox, RichTextBox sourceBoxRescript, VbScriptColorKey colorKey)
        {
            this.Listener = new RescriptControlListener();
            this._SourceBox = sourceBox;
            this._SourceBoxRescript = sourceBoxRescript;
            this._ColorKey = colorKey;
            this._CodeDocument = null;
            this._CodeDocumentRescript = null;
            this._LastVersion = -1;

            this.RewindCommand = new Command(this.Rewind, null);
            this.RescriptCommand = new Command(this.Rescript, null);
        }

        public void OnEnter()
        {
            if (this.Listener.IsBackgroundComplete() && this.Listener.Version != this._LastVersion)
            {
                this._CodeDocument = CodeDocument.Create(this.Listener.Code, this.Listener.Tokens, this._ColorKey, 14.0, VbScriptTextFunctions.CommentLines(this.Listener.Code), VbScriptColorKey.Green);
                this._CodeDocument.ApplyColors(VbScriptColorKey.Index);
                this._SourceBox.Document = this._CodeDocument.Document;
                this._LastVersion = this.Listener.Version;
            }
        }

        public void Rewind()
        {
            this._CodeDocumentRescript = null;
            this._SourceBoxRescript.Document = new FlowDocument();
        }

        public void Rescript()
        {
            var build = new StringBuilder();

            var rescript = this.Listener.Root.Map(p => p.ToVbScript(l => build.AppendLine(l)));

            if (rescript is Some<Unit>)
            {
                var rescriptCode = build.ToString();

                this._CodeDocumentRescript = CodeDocument.Create(rescriptCode, VbScript.Language.Lex(rescriptCode), this._ColorKey, 14.0, [], VbScriptColorKey.Green);
                this._CodeDocumentRescript.ApplyColors(VbScriptColorKey.Index);
                this._SourceBoxRescript.Document = this._CodeDocumentRescript.Document;
            }
        }

    }

}
