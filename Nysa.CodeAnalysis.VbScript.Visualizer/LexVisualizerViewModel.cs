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
using Nysa.Text.Lexing;

using Nysa.Logics;
using Nysa.Windows.Input;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public class LexVisualizerViewModel : NormalWindowViewModel
    {
        public String Source { get; private set; }
        public String Content { get; private set; }

        public ObservableCollection<TokenViewModel> Items { get; private set; }

        public new LexVisualizer Window => (LexVisualizer)base.Window;

        private TextRange? Selection;

        public LexVisualizerViewModel(LexVisualizer view, String source, String text, Token[] tokens)
            : base(view)
        {
            this.Source  = source;
            this.Content = text;

            this.Items = new ObservableCollection<TokenViewModel>();

            foreach (var token in tokens.Where(tkn => !tkn.Id.IsEqual(Nysa.Text.Identifier.Trivia)))
            {
                if (!string.IsNullOrWhiteSpace(token.Span.ToString()))
                    this.Items.Add(new TokenViewModel(this, token));
            }

            var paragraph = new Paragraph(new Run(this.Content)) {  };
            paragraph.FontFamily = new System.Windows.Media.FontFamily("Courier New");

            view._SourceText.Document = new FlowDocument();
            view._SourceText.Document.PageWidth = 2000;
            view._SourceText.Document.Blocks.Add(paragraph);
        }

        public void HighlightNode(TokenViewModel tokenViewModel)
        {
            if (this.Selection != null)
                this.Selection.ClearAllProperties();

            var start = this.Window._SourceText.Document.ContentStart;

            while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
                start = start.GetNextContextPosition(LogicalDirection.Forward);

            var startPos = start.GetPositionAtOffset(tokenViewModel.Position, LogicalDirection.Forward);
            var endPos = startPos.GetPositionAtOffset(tokenViewModel.Length, LogicalDirection.Forward);

            this.Selection = new TextRange(startPos, endPos);
            this.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
            // else if (node.Members.Count > 0)
            // {
            //     var first = node.Members.FirstOrNone(m => m.Position is Some<Int32>);
            //     var last  = node.Members.LastOrNone(m => m.Position is Some<Int32> && m.Length is Some<Int32>);

            //     if (   first                  is Some<TokenViewModel> firstVm
            //         && firstVm.Value.Position is Some<Int32>         someFirstPos
            //         && last                   is Some<TokenViewModel> lastVm
            //         && lastVm.Value.Position  is Some<Int32>         someLastPos
            //         && lastVm.Value.Length    is Some<Int32>         someLastLen )
            //     {
            //         var length = (someLastPos.Value + someLastLen.Value) - someFirstPos.Value;

            //         var start = this.Window._SourceText.Document.ContentStart;

            //         while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            //             start = start.GetNextContextPosition(LogicalDirection.Forward);

            //         var startPos = start.GetPositionAtOffset(someFirstPos.Value, LogicalDirection.Forward);
            //         var endPos = startPos.GetPositionAtOffset(length, LogicalDirection.Forward);

            //         this.Selection = new TextRange(startPos, endPos);
            //         this.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
            //     }
            // }
        }

    }

}
