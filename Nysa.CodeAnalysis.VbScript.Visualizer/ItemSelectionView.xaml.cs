using System;
using System.Collections.Generic;
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

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{
    /// <summary>
    /// Interaction logic for ItemSelection.xaml
    /// </summary>
    public partial class ItemSelectionView : NormalWindow
    {
        public ItemSelectionView()
        {
            InitializeComponent();
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.SearchBox.Text = String.Empty;
        }
    }
}
