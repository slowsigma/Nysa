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
                this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected));
                // if (this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected)))
                //     this.Parent.HighlightNode(this);
            }
        }

        public NodeViewModel(CodeVisualizerViewModel parent, ViewInfo basis)
            : base(parent)
        {
            this.Basis = basis;
            this.Position = Option.None;
            this.Length = Option.None;

            this.Members = new ObservableCollection<NodeViewModel>();

            foreach (var member in this.Basis.Children())
                this.Members.Add(new NodeViewModel(this.Parent, member));

            // var first = this.Basis.Select(node => node.First(), token => token.Span);
            // var last = this.Basis.Select(node => node.Last(), token => token.Span);

            // this.Position = first.Select(f => f.Position);
            // this.Length = first.Select(f => last.Select(l => (l.Position - f.Position) + l.Length));

            //foreach (var member in this.Basis.Select(node => node.Members, token => new NodeOrToken[] { }))
            //    this.Members.Add(new NodeViewModel(this.Parent, member));
        }
    }

}
