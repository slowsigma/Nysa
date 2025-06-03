// THIS IS GOING TO ESSENTIALLY BE THE SAME AS ParseTestingViewModel.cs BUT WILL FOCUS
// ON ALLOWING SOURCE TO CHANGE AS WELL AS FOR CONTROLLING AND RESETTING THE PARSING
// PROCESS THROUGH VbParseListener.cs



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Threading;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class LexingControlViewModel : ModelObject, IHighlight<TokenViewModel>
    {
        public ObservableCollection<TokenViewModel> Items { get; private set; }

        private Observable<String> _Content;
        private String _Source;

        public String Source
        {
            get { return this._Source; }
            set
            {
                if (!String.Equals(this._Source, value))
                {
                    this._Source = value;

                    var tokens = this._Source.Length > 0
                                 ? VbScript.Language.Lex(this._Source)
                                 : new Token[] { };

                    this.Items.Clear();

                    foreach (var token in tokens)
                        this.Items.Add(new TokenViewModel(this, token));

                    this.NotifyChanged(nameof(Source));
                }
            }
        }

        public void OnLeave()
        {
            this._Content.Value = this._Source;
        }

        public LexingControlViewModel(Observable<String> content)
        {
            this.Items = new ObservableCollection<TokenViewModel>();

            // this is used to update global view
            this._Content = content;
            // use the property setter to populate this.Items so tokens will populate
            this.Source = this._Content.Value;
        }

        public void Highlight(TokenViewModel item) { }
    }

}