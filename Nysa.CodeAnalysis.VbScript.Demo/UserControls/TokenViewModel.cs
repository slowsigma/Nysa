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
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class TokenViewModel : ViewModelObject<IHighlight<TokenViewModel>>
    {
        public Token    Basis       { get; private set; }
        public String   Text        => this.Basis.Span.ToString();
        public String   Id          => this.Basis.Id.ToString();
        public Int32    Position    => this.Basis.Span.Position;
        public Int32    Length      => this.Basis.Span.Length;

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get { return this._IsSelected; }
            set
            {
                if (this.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected)))
                    this.Parent.Highlight(this);
            }
        }

        public TokenViewModel(IHighlight<TokenViewModel> parent, Token basis)
            : base(parent)
        {
            this.Basis = basis;
        }
    }

}
