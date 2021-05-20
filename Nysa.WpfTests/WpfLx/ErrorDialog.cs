using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using Nysa.Logics;
using Nysa.Windows;

namespace Nysa.WpfLx
{

    public class ErrorDialog : DialogView
    {
        public static void Show(String displayMessage, Exception error, NormalWindow owner)
        {
            var window = Visuals.NormalWindow()
                                .Affected(w =>
                                {
                                    var copyButton = Visuals.DialogButton("Copy Details", () => { Clipboard.Clear(); Clipboard.SetText(error.ToString()); });

                                    w.Title = "Unexpected Error";

                                    var grid = Grids.RowGrid((new TextBlock()).Affected(b => { b.Text = displayMessage; b.TextWrapping = TextWrapping.Wrap; })
                                                                              .Styled(ViewResources.Items["BasicTextBlock"])
                                                                              .GridSet(0, 0)
                                                                              .Star(1),
                                                             Visuals.DialogButtonPanel(Visuals.DialogButton("Close", () => { w.Close(); }),
                                                                                       copyButton)
                                                                    .Auto());

                                    w.Content = (new DockPanel())
                                                .Styled(ViewResources.Items["WindowContentBlock"])
                                                .Affected(x => x.Children
                                                                .Add((new DockPanel())
                                                                     .Styled(ViewResources.Items["WindowContentAreaBlock"])
                                                                     .Affected(y => y.Children.Add(grid))));
                                }).Sized(340, 300);

            var dialog = window.Bound(vw => new ErrorDialog(vw, owner)).ViewModel;

            dialog.Window.Show();
        }

        private ErrorDialog(NormalWindow window, NormalWindow owner) : base(window, owner) { }
    }

}
