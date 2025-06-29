using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.ComponentModel;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public class BulletPointControlViewModel : ModelObject
    {
        public FlowDocument     Document    { get; private set; }

        public BulletPointControlViewModel(FlowDocumentScrollViewer scrollViewer, FlowDocument document)
        {
            this.Document   = document;

            scrollViewer.Document = this.Document;
        }
    }

}
