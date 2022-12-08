using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Nysa.Logics;
using Nysa.Text;

using Nysa.CodeAnalysis.VbScript.Semantics;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Boolean Flag(String[] args, String name)
            => args.Any(a => a.DataEquals($"-{name}") || a.DataEquals($"/{name}"));
        public static Option<String> Value(String[] args, String name)
            => args.SkipWhile(a => !a.DataEquals($"-{name}") && !a.DataEquals($"/{name}")).Skip(1).FirstOrNone();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var someFunc = "aFunc".ToFunctionSymbol(Option.None, 4);


            //var visualizerVVM = (new CodeVisualizer()).Bound(vw => new CodeVisualizerViewModel(vw));

            //visualizerVVM.View.Show();

            if (App.Value(e.Args, "path") is Some<String> somePath)
            {
                var itemSelectionVVM = (new ItemSelectionView()).Bound(vw => new ItemSelectionViewModel(somePath.Value, vw));
                itemSelectionVVM.View.Show();
            }
        }

    }
}
