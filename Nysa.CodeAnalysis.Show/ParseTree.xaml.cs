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
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.Show
{

    /// <summary>
    /// Interaction logic for ParseTree.xaml
    /// </summary>
    public partial class ParseTree : Window
    {
        public NodeViewModel Root { get; private set; }

        public ParseTree(Node root, String source)
        {
            this.Root = new NodeViewModel(root);
            InitializeComponent();

            this._CodeDisplay.Text = source;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var item = new TreeViewItem();

            item.DataContext    = this.Root;
            item.Header         = this.Root.Title;
            item.IsExpanded     = false;
            item.Selected       += Item_Selected;
            item.Expanded       += Item_Expanded;
            item.Items.Add("Placeholder");

            this._ParseTreeView.Items.Add(item);
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            var model = item?.DataContext as NodeViewModel;

            if (model != null && model.Position is Some<Int32>)
            {
                //this._CodeDisplay.SelectionStart = model.Position.Value;
                //this._CodeDisplay.SelectionLength = model.Length.Value;
            }
        }

        private void Item_Expanded(object sender, RoutedEventArgs e)
        {
            var item    = sender as TreeViewItem;
            var model   = item?.DataContext as NodeViewModel;

            if (model != null)
            {
                model.EnsureMembers();
                item.Items.Clear();

                foreach (var member in model.Members)
                {
                    var memberItem = new TreeViewItem();
                    memberItem.DataContext = member;
                    memberItem.Header = member.Title;
                    memberItem.IsExpanded = false;
                    memberItem.Selected += Item_Selected;
                    if (member.Basis.AsNode != null)
                    {
                        memberItem.Expanded += Item_Expanded;
                        memberItem.Items.Add("Placeholder");
                    }

                    item.Items.Add(memberItem);
                }

                item.Expanded -= Item_Expanded;
            }
        }
    }

}

