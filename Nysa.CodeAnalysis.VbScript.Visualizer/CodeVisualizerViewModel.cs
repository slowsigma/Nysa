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
using Nysa.Windows.Input;

using Dorata.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public class CodeVisualizerViewModel : NormalWindowViewModel
    {
        public String Source { get; private set; }
        public String Content { get; private set; }

        public ObservableCollection<NodeViewModel> Root { get; private set; }

        public new CodeVisualizer Window => (CodeVisualizer)base.Window;

        private TextRange? Selection;

        public CodeVisualizerViewModel(CodeVisualizer view, String source, String text, Suspect<Program> rootOrError)
            : base(view)
        {
            this.Source = source;

            this.Content = rootOrError.Match(r => text, e => e.Message);

            this.Root = new ObservableCollection<NodeViewModel>();

            var nvm = rootOrError.Match(pr => (NodeViewModel?)new NodeViewModel(this, pr), err => null);

            if (nvm != null)
                this.Root.Add(nvm);

            var paragraph = new Paragraph(new Run(this.Content)) {  };
            paragraph.FontFamily = new System.Windows.Media.FontFamily("Courier New");


            view._SourceText.Document = new FlowDocument();
            view._SourceText.Document.PageWidth = 2000;
            view._SourceText.Document.Blocks.Add(paragraph);

            //if (nvm != null)
            //    this.Colorize(nvm);
        }

        // private void Colorize(NodeViewModel node)
        // {
        //     var resWords = new List<NodeViewModel>();

        //     void Gather(List<NodeViewModel> nodes, NodeViewModel current)
        //     {
        //         if (current.Basis.IsToken && SymbolConstants.ReservedWords.Contains(current.Basis.AsToken.Id))
        //             nodes.Add(current);
        //         else
        //             current.Members.Affect(m => { Gather(nodes, m); });
        //     }

        //     Gather(resWords, node);

        //     var start = this.Window._SourceText.Document.ContentStart;

        //     while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
        //         start = start.GetNextContextPosition(LogicalDirection.Forward);


        //     foreach (var token in resWords.Reverse<NodeViewModel>())
        //     {
        //         var startPos = start.GetPositionAtOffset(token.Position.Value, LogicalDirection.Forward);
        //         var endPos = startPos.GetPositionAtOffset(token.Length.Value, LogicalDirection.Forward);

        //         var range = new TextRange(startPos, endPos);
        //         range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.DarkBlue));
        //     }
        // }

        // public void HighlightNode(NodeViewModel node)
        // {
        //     if (this.Selection != null)
        //         this.Selection.ClearAllProperties();

        //     if (node.Position.IsSome && node.Length.IsSome)
        //     {
        //         var start = this.Window._SourceText.Document.ContentStart;

        //         while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
        //             start = start.GetNextContextPosition(LogicalDirection.Forward);

        //         var startPos = start.GetPositionAtOffset(node.Position.Value, LogicalDirection.Forward);
        //         var endPos = startPos.GetPositionAtOffset(node.Length.Value, LogicalDirection.Forward);

        //         this.Selection = new TextRange(startPos, endPos);
        //         this.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
        //     }
        //     else if (node.Members.Count > 0)
        //     {
        //         var first = node.Members.FirstOrNone(m => m.Position.IsSome);
        //         var last  = node.Members.LastOrNone(m => m.Position.IsSome && m.Length.IsSome);

        //         if (first is Some<NodeViewModel> firstVm &&
        //             last  is Some<NodeViewModel> lastVm    )
        //         {
        //             var length = (lastVm.Value.Position.Value + lastVm.Value.Length.Value) - firstVm.Value.Position.Value;

        //             var start = this.Window._SourceText.Document.ContentStart;

        //             while (start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
        //                 start = start.GetNextContextPosition(LogicalDirection.Forward);

        //             var startPos = start.GetPositionAtOffset(firstVm.Value.Position.Value, LogicalDirection.Forward);
        //             var endPos = startPos.GetPositionAtOffset(length, LogicalDirection.Forward);

        //             this.Selection = new TextRange(startPos, endPos);
        //             this.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
        //         }
        //     }

        //     var offset = this.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
        //     this.Window._SourceScroll.ScrollToVerticalOffset(offset.Top);
        // }

    }

}
