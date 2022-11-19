using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.Show
{

    public class NodeViewModel
    {
        public NodeOrToken Basis { get; private set; }
        public ObservableCollection<NodeViewModel> Members { get; private set; }
        public String Title { get => this.Basis.Match(a => a.Symbol, b => b.Span.ToString()); }

        public Option<Int32> Position { get; private set; }
        public Option<Int32> Length { get; private set; }

        public NodeViewModel(NodeOrToken basis)
        {
            this.Basis = basis;
            this.Members = new ObservableCollection<NodeViewModel>();

            var first = this.Basis.Match(a => a.First(), b => b.Span.Some());
            var last  = this.Basis.Match(a => a.Last(),  b => b.Span.Some());

            this.Position = first.Map(f => f.Position);
            this.Length = first.Map(f => last is Some<TextSpan> someLast ? (someLast.Value.Position - f.Position) + someLast.Value.Length : 0);
        }

        public void EnsureMembers()
        {
            if (this.Basis.AsNode != null && this.Basis.AsNode.Members.Count > 0 && this.Members.Count == 0)
                foreach (var member in this.Basis.AsNode.Members.Cast<NodeOrToken>())
                    this.Members.Add(new NodeViewModel(member));
        }

    }

}