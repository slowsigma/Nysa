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

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class SemanticNodeViewModel : ViewModelObject<SemanticTreeControlViewModel>
    {
        public CodeNodeInfo Basis { get; private set; }
        public ObservableCollection<SemanticNodeViewModel> Members { get; private set; }
        public String Title => this.Basis.Title;
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

        public SemanticNodeViewModel(SemanticTreeControlViewModel parent, CodeNodeInfo basis)
            : base(parent)
        {
            this.Basis    = basis;
            this.Position = basis.Position;
            this.Length   = basis.Length;

            this.Members = new ObservableCollection<SemanticNodeViewModel>();

            foreach (var member in this.Basis.Members)
                this.Members.Add(new SemanticNodeViewModel(this.Parent, member));
        }
    }

}
