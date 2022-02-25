using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;
using Dorata.Text.Parsing;

namespace Nysa.CodeAnalysis.Visualizer
{

    public class NodeViewModel : ViewModelObject<CodeVisualizerViewModel>
    {
        public NodeOrToken Basis { get; private set; }
        public ObservableCollection<NodeViewModel> Members { get; private set; }
        public String Title => this.Basis.Select(node => node.Symbol, token => token.Span.Value);
        public Option<Int32> Position { get; private set; }
        public Option<Int32> Length { get; private set; }

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get { return this._IsSelected; }
            set
            {
                if (this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected)))
                    this.Parent.HighlightNode(this);
            }
        }

        public NodeViewModel(CodeVisualizerViewModel parent, NodeOrToken basis)
            : base(parent)
        {
            this.Basis = basis;
            this.Members = new ObservableCollection<NodeViewModel>();

            var first = this.Basis.Select(node => node.First(), token => token.Span);
            var last = this.Basis.Select(node => node.Last(), token => token.Span);

            this.Position = first.Select(f => f.Position);
            this.Length = first.Select(f => last.Select(l => (l.Position - f.Position) + l.Length));

            foreach (var member in this.Basis.Select(node => node.Members, token => new NodeOrToken[] { }))
                this.Members.Add(new NodeViewModel(this.Parent, member));
        }
    }

}
