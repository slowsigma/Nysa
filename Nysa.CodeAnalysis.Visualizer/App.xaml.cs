using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Nysa.CodeAnalysis.Visualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var visualizerVVM = (new CodeVisualizer()).Bound(vw => new CodeVisualizerViewModel(vw));

            //visualizerVVM.View.Show();


            var itemSelectionVVM = (new ItemSelectionView()).Bound(vw => new ItemSelectionViewModel(vw));
            itemSelectionVVM.View.Show();
        }

    }
}
