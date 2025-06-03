using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using IOPath = System.IO.Path;

using Nysa.CodeAnalysis.Documents;
using VbScript = Nysa.CodeAnalysis.VbScript;
using Nysa.Logics;
using System.CodeDom;

namespace Nysa.CodeAnalysis.VbScript.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var session = DemoSessionFunctions.CreateSession(IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        var demoContent = session.CreateContent();

        var demoVVM = (new DemoView()).Bound(vw => new DemoViewModel(demoContent.Pages, vw));
        demoVVM.View.Show();
    }

}

