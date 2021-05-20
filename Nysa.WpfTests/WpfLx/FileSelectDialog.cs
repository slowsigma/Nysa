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

    public class FileSelectDialog : DialogView
    {
        private static FrameworkElement ToUI(FileInfo fileInfo, Action<FileInfo> onFileSelect)
        {
            var button  = Visuals.BasicRadioButton("files", false, null, fileInfo.FullName);
            var nameBox = fileInfo.Name.TextBox(true, true);

            button.Checked += (s, e) => { onFileSelect(fileInfo); };
            nameBox.PreviewKeyDown += (s, e) => { if (e.Key == Key.Space) button.IsChecked = true; };

            return Grids.RowGrid(Grids.ColumnGrid(button.Auto("selected"), nameBox.Star(1, "names")).Auto());
        }

        public static String? Show(String title, NormalWindow owner)
        {
            var window       = Visuals.NormalWindow();
            var result       = (String?)null;
            var folder       = (String?)null;
            var files        = new List<FileInfo>();
            var folderSelect = FolderPickerControl.Create();
            var okButton     = "OK".DialogButton(() => { window.Close(); }).Sized(60.0, 26.0);
            var cancelButton = "Cancel".DialogButton(() => { result = (String?)null; window.Close(); }).Sized(60.0, 26.0);

            void Selected(FileInfo fileInfo)
            {
                if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    folderSelect.DirectoryCursor.Reset(fileInfo.Directory.FullName);
                    folderSelect.DirectoryCursor.Append(fileInfo.Name);
                    folderSelect.DirectoryCursor.TakeCompletion();
                }
                else
                {
                    result = fileInfo.FullName;
                    okButton.IsEnabled = !String.IsNullOrWhiteSpace(fileInfo.FullName);
                }
            }

            var fileItems    = files.Select(i => (Item: i, Visual: ToUI(i, Selected))).ToList();

            var (filesVisual,
                 filesRefresh) = Visuals.ItemsManagementControl(fileItems.ItemsVisual(), "Files", null);

            folderSelect.DirectoryCursor.CurrentChanged += (s, e) =>
            {
                result = null;
                files.Clear();
                fileItems.Clear();
                okButton.IsEnabled = false;

                if (Directory.Exists(folderSelect.DirectoryCursor.Current))
                {
                    folder = folderSelect.DirectoryCursor.Current;

                    var path = folder.EndsWith('\\') ? folder : String.Concat(folder, '\\');

                    files.AddRange(Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly)
                                            .Select(f => new FileInfo(f)));
                    fileItems = files.Select(i => (Item: i, Visual: ToUI(i, Selected))).ToList();
                }

                filesRefresh(fileItems.ItemsVisual());
            };

            window.Affect(w =>
            {
                w.Title = title;

                var folderArea = Grids.RowGrid("Folder".TextBoxLabel().Star(),
                                               folderSelect.Star());

                var okCancelPanel = Visuals.DialogButtonPanel(okButton,
                                                              cancelButton);

                w.Content = Grids.RowGrid(Visuals.ViewContentBlock(folderArea).WithSeparator().Auto(),
                                          Visuals.ViewContentBlock(filesVisual).WithSeparator().Star(),
                                          Visuals.ViewContentBlock(okCancelPanel).Auto())
                                 .ViewContent();

                w.Sized(340, 420);
            });

            var dialog = window.Bound(vw => new FileSelectDialog(vw, owner)).ViewModel;

            dialog.Window.ShowDialog();

            return result;
        }

        private FileSelectDialog(NormalWindow window, NormalWindow owner) : base(window, owner) { }
    }

}