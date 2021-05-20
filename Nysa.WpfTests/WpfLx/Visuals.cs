using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

using Nysa.Logics;
using Nysa.Windows;

namespace Nysa.WpfLx
{

    public static class Visuals
    {
        public static Label? ToolTipLabel(this String? content)
            => String.IsNullOrWhiteSpace(content)
               ? null
               : new Label() { Content = content };


        public static TextBox Required(this TextBox textBox)
        {
            return textBox.Affected(b =>
            {
                b.TextChanged += (s, e) =>
                {
                    if (String.IsNullOrWhiteSpace(b.Text))
                        b.BorderBrush = ViewResources.InvalidBorderBrush;
                    else
                        b.BorderBrush = ViewResources.NormalBorderBrush;
                };

                // initial color
                if (String.IsNullOrWhiteSpace(b.Text))
                    b.BorderBrush = ViewResources.InvalidBorderBrush;
                else
                    b.BorderBrush = ViewResources.NormalBorderBrush;
            });
        }

        public static PasswordBox Required(this PasswordBox passwordBox)
        {
            return passwordBox.Affected(b =>
            {
                if (String.IsNullOrWhiteSpace(b.Password))
                    b.BorderBrush = ViewResources.InvalidBorderBrush;
                else
                    b.BorderBrush = ViewResources.NormalBorderBrush;

                // initial color
                if (String.IsNullOrWhiteSpace(b.Password))
                    b.BorderBrush = ViewResources.InvalidBorderBrush;
                else
                    b.BorderBrush = ViewResources.NormalBorderBrush;
            });
        }

        public static CheckBox BasicCheckBox(Boolean isChecked = false, String? tag = null, String? toolTip = null)
            => (new CheckBox()).Affected(b =>
            {
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalAlignment = HorizontalAlignment.Center;
                b.IsThreeState = false;
                b.IsChecked = isChecked;
                if (!String.IsNullOrWhiteSpace(tag))
                    b.Tag = tag;

                b.ToolTip = toolTip.ToolTipLabel();
            });

        public static RadioButton BasicRadioButton(String groupName, Boolean isChecked = false, String? tag = null, String? toolTip = null)
            => (new RadioButton()).Affected(b =>
            {
                b.GroupName = groupName;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalAlignment = HorizontalAlignment.Center;
                b.IsThreeState = false;
                b.IsChecked = isChecked;
                if (!String.IsNullOrWhiteSpace(tag))
                    b.Tag = tag;

                b.ToolTip = toolTip.ToolTipLabel();
            });

        public static Setter AsSetter(this DependencyProperty @this, Object value)
            => new Setter(@this, value);

        public static T WithProperty<T>(this T @this, DependencyProperty property, Object value)
            where T : UIElement
            => @this.Affected(e => e.SetValue(property, value));

        public static Button DialogButton(this String @this, Action onClick)
            => (new Button()).Affected(b => { b.Content = @this; b.Click += (s, a) => onClick(); })
                             .Styled(ViewResources.Items["DialogButton"]);

        public static Button DialogButton(this String @this, Func<Task> onClickAsync)
            => (new Button()).Affected(b => { b.Content = @this; b.Click += async (s, a) => await onClickAsync(); })
                             .Styled(ViewResources.Items["DialogButton"]);

        public static Button DialogButton(this String @this)
            => (new Button()).Affected(b => { b.Content = @this; })
                             .Styled(ViewResources.Items["DialogButton"]);

        public static Button DialogButton(this String @this, String? toolTip)
            => (new Button()).Affected(b => { b.Content = @this; b.ToolTip = toolTip.ToolTipLabel(); })
                             .Styled(ViewResources.Items["DialogButton"]);

        public static Label TextBoxLabel(this String caption, String? toolTip = null)
            => (new Label()).Affected(l => { l.Content = caption; })
                            .Styled(ViewResources.Items["TextBoxLabel"])
                            .Affected(m => { m.ToolTip = toolTip.ToolTipLabel(); });

        public static Label DataHeader(this String caption)
            => (new Label()).Affected(l => { l.Content = caption; })
                            .Styled(ViewResources.DataHeaderLabelStyle);

        public static TextBox TextBox(this String? content, Boolean isEnabled = true, Boolean isReadOnly = false, Action<String>? onChange = null)
            => (new TextBox()).Affected(b =>
            {
                b.Text = content == null ? String.Empty : content;
                b.IsEnabled = isEnabled; b.IsReadOnly = isReadOnly;
                if (!isReadOnly && onChange != null)
                    b.TextChanged += (s, e) => { onChange(b.Text); };
            }).Styled(ViewResources.Items["BasicTextBox"]);

        public static PasswordBox PasswordBox(this String? content, Action<String>? onChange = null)
            => (new PasswordBox()).Affected(b =>
            {
                b.Password = content;
                if (onChange != null)
                    b.PasswordChanged += (s, e) => { onChange(b.Password); };
            }).Styled(ViewResources.Items["PasswordBox"]);

        public static TextBlock TextBlock(this String content, Boolean wrapText)
            => (new TextBlock()).Affected(b => { b.Text = content; if (wrapText) b.TextWrapping = TextWrapping.Wrap; })
                                .Styled(ViewResources.Items["BasicTextBlock"]);

        public static StackPanel PlainPanel(params UIElement[] content)
            => (new StackPanel()).WithChildren(content);

        public static StackPanel UpDownPanel(params UIElement[] content)
            => (new StackPanel()).Affected(p => p.Orientation = Orientation.Vertical)
                                 .WithChildren(content);

        public static StackPanel LeftRightPanel(params UIElement[] content)
            => (new StackPanel()).Affected(p => p.Orientation = Orientation.Horizontal)
                                 .WithChildren(content);

        public static StackPanel DialogButtonPanel(params UIElement[]? buttons)
            => (new StackPanel()).Affected(p =>
            {
                p.Orientation = Orientation.Horizontal;
                p.FlowDirection = FlowDirection.RightToLeft;
                p.Margin = new Thickness(12, 12, 12, 0);
            }).WithChildren(buttons);

        public static StackPanel IndicatorIcon(String iconName, String? toolTip)
            => (new StackPanel()).Affected(p =>
            {
                p.Orientation = Orientation.Horizontal;
                p.Children.Add((new Image() { Source = (DrawingImage)ViewResources.Items[iconName] }).Sized(20, 20));
                p.ToolTip = toolTip.ToolTipLabel();
            });

        public static T Hidden<T>(this T @this)
            where T : UIElement
            => @this.Affected(e => { e.Visibility = Visibility.Hidden; });
        public static T Shown<T>(this T @this)
            where T : UIElement
            => @this.Affected(e => { e.Visibility = Visibility.Visible; });
        public static T Collapsed<T>(this T @this)
            where T : UIElement
            => @this.Affected(e => { e.Visibility = Visibility.Collapsed; });


        public static Button IconTextPopupButton(String iconName, String caption)
            => (new Button()).Styled(ViewResources.Items["IconTextPopupButton"])
                             .Affected(b =>
                             {
                                 b.Content = (new StackPanel()).Affected(p =>
                                 {
                                     p.Orientation = Orientation.Horizontal;
                                     p.Children.Add((new Image() { Source = (DrawingImage)ViewResources.Items[iconName] }).Sized(24, 24));
                                     p.Children.Add((new TextBlock() { Text = caption }).Styled(ViewResources.Items["IconTextButtonText"]));
                                 });
                             });

        public static Grid PopupMenuGrid(params Button[] buttons)
            => (new Grid()).Affected(g =>
            {
                var border = (new Border()).Affected(b =>
                {
                    b.BorderThickness = new Thickness(1);
                    b.Margin = new Thickness(5);
                    b.Effect = new DropShadowEffect() { BlurRadius = 10, Opacity = 0.8, ShadowDepth = 0, Color = Colors.LightGray };
                });
                g.Styled(ViewResources.Items["PopupMenuGrid"]);
                g.Children.Add(border);
                border.Child = Grids.RowGrid(buttons.Select(x => x.Auto()));
            });

        public static (Button Button, Popup Popup) PopupMenuButton(String iconName, params Button[] iconTextPopupButtons)
            => (new Popup()).Affected(p =>
               {
                   p.SetValue(Popup.AllowsTransparencyProperty, true);
                   p.SetValue(Popup.StaysOpenProperty, false);
                   p.Child = Visuals.PopupMenuGrid(iconTextPopupButtons);
               }).Make(pop => ((new Button()).Affected(b => b.Style = (new Style(typeof(Button)))
                                                                      .WithBorderThickness(0)
                                                                      .WithBackground(Colors.Transparent.AsBrush())
                                                                      .WithPadding(3))
                                             .Sized(24, 24)
                                             .Containing((new StackPanel()).Containing(new Image() { Source = (DrawingImage)ViewResources.Items[iconName] }, pop)),
                               pop)
               );

        public static T WithChildren<T>(this T @this, params UIElement[]? children)
            where T : Panel
            => @this.Affected(p => children.Affect(c => { p.Children.Add(c); }));

        private static Style IconButtonStyle(String iconName)
            => typeof(Button).StyleTarget()
                             .WithSetter(Button.ContentProperty.AsSetter(new Image() { Source = (DrawingImage)ViewResources.Items[iconName] }))
                             .WithSetter(Button.BorderThicknessProperty.AsSetter(new Thickness(0)))
                             .WithSetter(Button.BackgroundProperty.AsSetter(Colors.Transparent.AsBrush()))
                             .WithSetter(Button.PaddingProperty.AsSetter(new Thickness(3)));

        public static Button IconButton(String iconName, String? toolTip = null)
            => (new Button()).Styled(IconButtonStyle(iconName)).Sized(20, 20).Affected(b => { b.ToolTip = toolTip.ToolTipLabel(); });

        public static SolidColorBrush AsBrush(this Color color)
            => new SolidColorBrush(color);

        public static NormalWindow NormalWindow()
            => (new NormalWindow()).Affected(w =>
            {
                w.Resources
                 .MergedDictionaries
                 .Add(ViewResources.Items);

                w.Style = (Style)ViewResources.Items["NormalWindowStyle"];
            });

        public static T Containing<T>(this T control, Object content)
            where T : ContentControl
            => control.Affected(c => { c.Content = content; });
        public static T Containing<T>(this T panel, params UIElement[] children)
            where T : Panel
            => panel.Affected(p => { children.Affect(c => { p.Children.Add(c); }); });
        public static T WithContent<T>(this T element, Object content)
            where T : ContentControl
            => element.Affected(e => e.Content = content);

        public static T Sized<T>(this T @this, Double width, Double height)
            where T : FrameworkElement
            => @this.Affected(v => { v.Width = width; v.Height = height; });

        public static T Padded<T>(this T @this, Thickness padding)
            where T : Control
            => @this.Affected(c => c.Padding = padding);
        public static T Padded<T>(this T @this, Double leftRight, Double topBottom)
            where T : Control
            => @this.Affected(c => c.Padding = new Thickness(leftRight, topBottom, leftRight, topBottom));
        public static T Padded<T>(this T @this, Double left, Double top, Double right, Double bottom)
            where T : Control
            => @this.Affected(c => c.Padding = new Thickness(left, top, right, bottom));

        public static T Margined<T>(this T @this, Double left, Double top, Double right, Double bottom)
            where T : Control
            => @this.Affected(c => c.Margin = new Thickness(left, top, right, bottom));

        public static Grid Margined(this Grid @this, Double left, Double top, Double right, Double bottom)
            => @this.Affected(c => c.Margin = new Thickness(left, top, right, bottom));

        public static NormalWindow Sized(this NormalWindow @this, Double width, Double height)
            => @this.Affected(w =>
            {
                w.Resources.MergedDictionaries.Add(ViewResources.Items);
                w.Width = width; 
                w.Height = height;
            });

        public static DockPanel ViewContentBlock(params UIElement[] children)
            => (new DockPanel()).Affected(p => { children.Affect(c => { p.Children.Add(c); }); })
                                .Styled(ViewResources.Items["WindowContentAreaBlock"]);

        public static UIElement WithSeparator(this DockPanel content, SeparatorPosition position = SeparatorPosition.below)
            => (new Border()).Affected(b =>
            {
                b.BorderThickness =   position == SeparatorPosition.below ? new Thickness(0, 0, 0, 5)
                                    : position == SeparatorPosition.above ? new Thickness(0, 5, 0, 0)
                                    : position == SeparatorPosition.after ? new Thickness(0, 0, 5, 0)
                                    :                                       new Thickness(5, 0, 0, 0);
                b.BorderBrush = Colors.WhiteSmoke.AsBrush();
                b.Child = content;
            });

        public static DockPanel ViewContent(this UIElement child)
            => (new DockPanel()).Affected(d => { d.Children.Add(child); })
                                .Styled(ViewResources.Items["WindowContentBlock"]);

        public static (UIElement Visual, TextBox InputBox) SearchBox(Action<String> filterStringChange)
        {
            var boxShadow = (new TextBlock()).Affected(b =>
            {
                b.GridSet(0, 0);
                b.Text = "Search...";
                b.Style = typeof(TextBlock)
                    .StyleTarget()
                    .WithFont("Segoe UI", 16, null, FontStyles.Italic)
                    .WithVisibility(Visibility.Visible)
                    .WithMargin(10, 0, 0, 0)
                    .WithForeground(Colors.Gray.AsBrush())
                    .WithSetter(Control.IsHitTestVisibleProperty.AsSetter(false));
                b.SetValue(Grid.ZIndexProperty, 0);
            });
            var boxText = (new TextBox()).Affected(t =>
            {
                t.GridSet(0, 0);
                t.Style = (Style)ViewResources.Items["SearchTextBox"];
                t.TextChanged += new TextChangedEventHandler((s, a) =>
                {
                    boxShadow.Visibility = t.Text.Length == 0 ? Visibility.Visible : Visibility.Collapsed;
                    filterStringChange(t.Text);
                });
                t.KeyUp += new KeyEventHandler((s, a) =>
                {
                    if (a.Key == Key.Escape)
                        t.Text = String.Empty;
                });
                t.SetValue(Grid.ZIndexProperty, 1);
                t.Visibility = Visibility.Visible;
                t.Focusable = true;
                t.IsTabStop = true;
            });
            var visual = (new Grid()).Affected(g =>
            {
                g.Background = Color.FromRgb(0xEE, 0xEE, 0xEE).AsBrush();
                g.Children.Add(boxText);
                g.Children.Add(boxShadow);
            });

            return (visual, boxText);
        }

        public delegate void ItemsRefresh(UIElement content);
        public delegate TextBox ItemsAndSearchRefresh(UIElement content, Action<String> searchFilterChange);

        public static (NormalWindow Window, TextBox SearchBox, Action<UIElement> itemsContentChange) ItemsManagementWindow(Action<String> searchFilterStringChange, UIElement[] headerItems, UIElement[]? footerItems = null, Int32 width = 800, Int32 height = 600)
        {
            var window = NormalWindow().Sized(width, height);

            var (searchVisual, searchBox) = SearchBox(searchFilterStringChange);
            var searchContent = (new DockPanel()).Affected(p => { p.Children.Add(searchVisual); });

            // item search and add-item header
            var header = ViewContentBlock(Grids.ColumnGrid(Return.Enumerable(searchContent.Star(2)).Concat(headerItems.Select(h => h.Auto())))).WithSeparator();

            var scroller = new ScrollViewer() { };

            var content = ViewContentBlock(scroller);

            var footer = ViewContentBlock(Visuals.DialogButtonPanel(footerItems)).WithSeparator(SeparatorPosition.above);

            var grid = (footerItems == null || footerItems.Length == 0)
                       ? Grids.RowGrid(header.Auto(), content.Star(1))
                       : Grids.RowGrid(header.Auto(), content.Star(1), footer.Auto());

            window.Content = (new DockPanel()).Affected(d =>
            {
                d.Style = (Style)ViewResources.Items["WindowContentBlock"];
                d.Children.Add(grid);
            });

            return (window, searchBox, itemsContent =>
            {
                scroller.Content = itemsContent;
            });
        }

        public static FrameworkElement ItemsVisual<T>(this List<(T Item, FrameworkElement Visual)> @this, Boolean isSharedSizeScope = false)
            => (new StackPanel()).Affected(v =>
            {
                if (isSharedSizeScope)
                    v.SetValue(Grid.IsSharedSizeScopeProperty, true);

                v.Orientation = Orientation.Vertical;
                @this.Affect(i => { v.Children.Add(i.Visual); });
            });

        public static FrameworkElement ItemsVisual(this IEnumerable<FrameworkElement> @this, Boolean isSharedSizeScope = false)
            => (new StackPanel()).Affected(v =>
            {
                if (isSharedSizeScope)
                    v.SetValue(Grid.IsSharedSizeScopeProperty, true);

                v.Orientation = Orientation.Vertical;
                @this.Affect(i => { v.Children.Add(i); });
            });

        public static (FrameworkElement Control, ItemsRefresh Refresh) ItemsManagementControl(UIElement itemsContent, String label, UIElement? itemHeaders, params Button[] actionButtons)
        {
            var lblVis = (new Label()).Affected(l => { l.Content = label; }).Styled(ViewResources.DataHeaderLabelStyle) as UIElement;
            var header = Grids.ColumnGrid(Return.Enumerable(lblVis.Star(1)).Concat(actionButtons.Select(a => a.Auto())))
                              .Margined(2, 2, 2, 2);

            var scroller = new ScrollViewer() { Content = itemsContent };
            var grid     = (itemHeaders == null)
                           ? Grids.RowGrid(header.Auto(), scroller.Star(1))
                           : Grids.RowGrid(header.Auto(), itemHeaders.Auto(), scroller.Star(1));

            grid.SetValue(Grid.IsSharedSizeScopeProperty, true);

            return (grid, (newContent) =>
            {
                scroller.Content = newContent;
                grid.SetValue(Grid.IsSharedSizeScopeProperty, true);
                grid.UpdateLayout();
            });
        }
    }

}
