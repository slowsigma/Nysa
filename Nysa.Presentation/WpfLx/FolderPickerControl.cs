using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using InputKey = System.Windows.Input.Key;

using Nysa.Logics;
using Nysa.IO;

namespace Nysa.WpfLx
{

    public class FolderPickerControl : UserControl
    {
        public static FolderPickerControl Create(String? startPath = null, Style? textBoxStyle = null)
        {
            var dirPick     = new DirectoryCursor(startPath ?? String.Empty);
            var dirChng     = false;
            var completion  = Visuals.TextBox(null);
            var input       = Visuals.TextBox(null);
            var control     = new FolderPickerControl(dirPick);

            void ChangeText(String newInput, String newCompletion)
            {
                dirChng = true;
                input.Text = newInput;
                dirChng = false;
                completion.Text = newCompletion;

                if (input.CaretIndex < input.Text.Length)
                    input.CaretIndex = input.Text.Length;
            }

            completion.Affect(c =>
            {
                if (textBoxStyle != null)
                    c.Style = textBoxStyle;

                c.VerticalContentAlignment = VerticalAlignment.Center;
                c.SetValue(Panel.ZIndexProperty, 0);
                c.IsTabStop = false;
                c.IsEnabled = false;
                c.Text = dirPick.Completion;
            });

            input.Affect(i =>
            {
                if (textBoxStyle != null)
                    i.Style = textBoxStyle;

                i.VerticalContentAlignment = VerticalAlignment.Center;
                i.SetValue(Panel.ZIndexProperty, 1);
                i.Background = Colors.Transparent.AsBrush();
                i.Text = String.Empty;
                i.TextChanged += (s, e) =>
                {
                    if (dirChng)
                        return;

                    var change = e.Changes.FirstOrDefault();

                    if (change != null)
                    {
                        if (change.AddedLength > 0)
                        {
                            dirPick.Append(i.Text.Substring(change.Offset, change.AddedLength));
                            ChangeText(dirPick.Search, dirPick.Completion);
                        }
                    }
                };
                i.KeyUp += (s, e) =>
                {
                    if (e.Key == InputKey.Enter)
                    {
                        dirPick.TakeCompletion();
                        ChangeText(dirPick.Search, dirPick.Completion);
                        e.Handled = true;
                    }
                    else if (e.Key == InputKey.Back)
                    {
                        dirPick.Backspace();
                        ChangeText(dirPick.Search, dirPick.Completion);
                        e.Handled = true;
                    }
                    else if (e.Key == InputKey.Down || e.Key == InputKey.Up)
                    {
                        dirPick.MoveCompletion(e.Key == InputKey.Down);
                        completion.Text = dirPick.Completion;
                        e.Handled = true;
                    }
                };
            });

            dirPick.CurrentChanged += (s, a) =>
            {
                ChangeText(dirPick.Search, dirPick.Completion);
                control.SetValue(Control.ToolTipProperty, dirPick.Current);
            };

            control.Affect(c =>
            {
                c.Content = (new Grid()).Affected(g =>
                {
                    g.Children.Add(completion);
                    g.Children.Add(input);
                });
            });

            return control;
        }

        // instance members
        public DirectoryCursor DirectoryCursor { get; private set; }

        private FolderPickerControl(DirectoryCursor directoryCursor)
        {
            this.DirectoryCursor = directoryCursor;
        }
    }

}
