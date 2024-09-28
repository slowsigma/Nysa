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

using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public class NodeViewModel : ViewModelObject<CodeVisualizerViewModel>
    {
        public ViewInfo Basis { get; private set; }
        public ObservableCollection<NodeViewModel> Members { get; private set; }
        public String Title => this.Basis.Title;
        public Option<Int32> Position { get; private set; }
        public Option<Int32> Length { get; private set; }

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get { return this._IsSelected; }
            set
            {
                //this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected));

                if (this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected)))
                    this.Parent.HighlightNode(this);
            }
        }

        public NodeViewModel(CodeVisualizerViewModel parent, ViewInfo basis)
            : base(parent)
        {
            this.Basis    = basis;
            this.Position = Option.None;
            this.Length   = Option.None;

            this.Members = new ObservableCollection<NodeViewModel>();

            foreach (var member in this.Basis.Children())
                this.Members.Add(new NodeViewModel(this.Parent, member));

            var bounds = this.Basis.Node.TokenBounds();

            if (bounds.Start.HasValue)
            {
                this.Position = bounds.Start.Value.Span.Position.Some();

                if (bounds.End.HasValue)
                    this.Length = ((bounds.End.Value.Span.Position + bounds.End.Value.Span.Length) - bounds.Start.Value.Span.Position).Some();
            }
        }
    }

}
