using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Windows;

using Nysa.WpfLx;

namespace Nysa.WpfTests
{

    public class FolderPickerDialog : DialogView
    {
        public static String? Show(App app, String title)
        {
            var window          = Visuals.NormalWindow();
            var folderPicker    = FolderPickerControl.Create(ViewResources.BasicTextBoxStyle);
            var result          = (String)null;

            folderPicker.DirectoryCursor.CurrentChanged += (s, e) =>
            {
            };

            window.Affect(w =>
            {
                w.Title = title;

                var okCancelPanel = Visuals.DialogButtonPanel("OK".DialogButton(() => { w.Close(); }).Sized(60.0, 26.0),
                                                              "Cancel".DialogButton(() => { result = null; w.Close(); }).Sized(60.0, 26.0));

                w.Content = Grids.RowGrid(Visuals.ViewContentBlock(folderPicker).WithSeparator().Auto(),
                                          Visuals.ViewContentBlock(okCancelPanel).Auto())
                                 .ViewContent();

                w.Sized(720, 540);
            });

            var dialog = window.Bound(vw => new FolderPickerDialog(vw)).ViewModel;

            dialog.Window.ShowDialog();

            return result;
        }

        // instance members
        private FolderPickerDialog(NormalWindow window) : base(window) { }
    }

}