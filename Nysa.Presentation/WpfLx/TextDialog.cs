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

    public class TextDialog : DialogView
    {
        public static void Show(String title, String text, NormalWindow owner)
        {
            var window = Visuals.NormalWindow()
                                .Affected(w =>
                                {
                                    w.Title = title;

                                    var grid = Grids.RowGrid((new TextBox()).Styled(ViewResources.Items["BasicTextBox"])
                                                                            .Affected(b => { b.Text = text; b.TextWrapping = TextWrapping.Wrap; b.IsReadOnly = true; b.VerticalAlignment = VerticalAlignment.Top; })
                                                                            .GridSet(0, 0)
                                                                            .Star(1),
                                                             Visuals.DialogButtonPanel(Visuals.DialogButton("Ok", () => { w.Close(); }))
                                                                    .Auto());

                                    w.Content = (new DockPanel())
                                                .Styled(ViewResources.Items["WindowContentBlock"])
                                                .Affected(x => x.Children
                                                                .Add((new DockPanel())
                                                                     .Styled(ViewResources.Items["WindowContentAreaBlock"])
                                                                     .Affected(y => y.Children.Add(grid))));

                                }).Sized(440, 320);

            var dialog = window.Bound(vw => new TextDialog(vw, owner)).ViewModel;

            dialog.Window.ShowDialog();
        }

        private TextDialog(NormalWindow window, NormalWindow owner) : base(window, owner) { }
    }

}
