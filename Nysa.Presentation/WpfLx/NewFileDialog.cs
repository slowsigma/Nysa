using System;
using System.Collections.Generic;
using System.IO;
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

    public class NewFileDialog : DialogView
    {

        public static String? Show(String title, String defaultPath, String extension, NormalWindow owner)
        {
            var window       = Visuals.NormalWindow();
            var result       = (String?)null;
            var folder       = (String?)null;
            var nameBox      = (String.Concat("newFile", extension)).TextBox();
            var folderSelect = FolderPickerControl.Create(defaultPath);
            var okButton     = "OK".DialogButton(() => { window.Close(); }).Sized(60.0, 26.0);
            var cancelButton = "Cancel".DialogButton(() => { result = (String?)null; window.Close(); }).Sized(60.0, 26.0);

            folderSelect.DirectoryCursor.CurrentChanged += (s, e) =>
            {
                result = null;
                okButton.IsEnabled = false;

                if (Directory.Exists(folderSelect.DirectoryCursor.Current))
                {
                    folder = folderSelect.DirectoryCursor.Current;

                    result = String.IsNullOrWhiteSpace(nameBox.Text) ? Path.Combine(folder, nameBox.Text) : null;

                    okButton.IsEnabled = result != null && !File.Exists(result);
                }
            };

            window.Affect(w =>
            {
                w.Title = title;

                var folderArea = Grids.RowGrid("Folder".TextBoxLabel().Star(),
                                               folderSelect.Star());

                var okCancelPanel = Visuals.DialogButtonPanel(okButton,
                                                              cancelButton);

                w.Content = Grids.RowGrid(Visuals.ViewContentBlock(folderArea).WithSeparator().Auto(),
                                          Visuals.ViewContentBlock(Grids.RowGrid("File Name".TextBoxLabel().Auto(), nameBox.Auto())).WithSeparator().Star(),
                                          Visuals.ViewContentBlock(okCancelPanel).Auto())
                                 .ViewContent();

                w.Sized(340, 420);
            });

            var dialog = window.Bound(vw => new NewFileDialog(vw, owner)).ViewModel;

            dialog.Window.ShowDialog();

            return result;
        }

        private NewFileDialog(NormalWindow window, NormalWindow owner) : base(window, owner) { }
    }

}