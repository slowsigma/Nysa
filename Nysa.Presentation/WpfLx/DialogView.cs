using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using Nysa.Windows;

namespace Nysa.WpfLx
{

    public class DialogView : NormalWindowViewModel
    {
        public DialogView(NormalWindow window, NormalWindow owner)
            : base(window)
        {
            this.ShowWindowMaximizeRestore  = false;
            this.ShowWindowMinimize         = false;

            window.WindowStartupLocation    = WindowStartupLocation.CenterOwner;
            window.Owner                    = owner;
        }
    }

}
