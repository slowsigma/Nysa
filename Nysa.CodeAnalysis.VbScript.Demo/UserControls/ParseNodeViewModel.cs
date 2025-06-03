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
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class ParseNodeViewModel : ViewModelObject<ParseTreeControlViewModel>
    {
        public NodeOrToken Basis { get; private set; }
        public ObservableCollection<ParseNodeViewModel> Members { get; private set; }
        public String Title =>   this.Basis.AsNode != null ? this.Basis.AsNode.Symbol
                               : this.Basis.AsToken != null ? this.Basis.AsToken.Value.Span.ToString()
                               : "__error__";
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

        public ParseNodeViewModel(ParseTreeControlViewModel parent, NodeOrToken basis)
            : base(parent)
        {
            this.Basis    = basis;
            this.Position = Option.None;
            this.Length   = Option.None;

            this.Members = new ObservableCollection<ParseNodeViewModel>();

            if (this.Basis.AsNode != null)
            {
                foreach (var member in this.Basis.AsNode.Members)
                    this.Members.Add(new ParseNodeViewModel(this.Parent, member));

                var bounds = this.Basis.AsNode.OrderedTokens().ToArray();

                if (bounds.Length > 0)
                {
                    this.Position = bounds[0].Span.Position.Some();

                    this.Length = bounds.Length > 1
                                  ? ((bounds[bounds.Length - 1].Span.Position + bounds[bounds.Length - 1].Span.Length) - bounds[0].Span.Position).Some()
                                  : bounds[0].Span.Length.Some();
                }
            }
            else if (this.Basis.AsToken != null)
            {
                this.Position = this.Basis.AsToken.Value.Span.Position.Some();
                this.Length = this.Basis.AsToken.Value.Span.Length.Some();
            }
        }
    }

}
