using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Nysa.Logics;
using Nysa.WpfLx;

namespace Nysa.WpfTests
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var pick = FolderPickerDialog.Show(this, "Folder Picker");
        }

    }

}
