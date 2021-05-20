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

    public class InfoDialog : DialogView
    {
        public static Boolean Show(String title, String message, NormalWindow owner)
        {
            var result = false;

            var window = Visuals.NormalWindow()
                                .Affected(w =>
                                {
                                    w.Title = title;

                                    var messageBlock = message.TextBlock(true);

                                    var okCancelPanel = Visuals.DialogButtonPanel("OK".DialogButton(() => { result = false; w.Close(); }).Sized(60.0, 26.0));

                                    var grid = Grids.RowGrid(messageBlock.Star(1),
                                                             okCancelPanel.Auto());

                                    w.Content = Visuals.ViewContent(Visuals.ViewContentBlock(grid));
                                }).Sized(340, 220);

            var dialog = window.Bound(vw => new InfoDialog(vw, owner)).ViewModel;

            dialog.Window.ShowDialog();

            return result;
        }

        private InfoDialog(NormalWindow window, NormalWindow owner) : base(window, owner) { }
    }

}
