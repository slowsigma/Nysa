using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Nysa.Scheduling.Calendars;

namespace Nysa.Scheduling.RuleVisualizer
{

    public static class VisualTransforms
    {
        private static UIElement DayHeader()
        {
            UIElement dayLabel(String day, Int32 index)
            {
                var label = new Label() { HorizontalContentAlignment = HorizontalAlignment.Center, Content = day };
                label.SetValue(Grid.ColumnProperty, index);
                return label;
            };

            var header = new Grid();
            header.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var dayPrefix = new String[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            for (Int32 c = 0; c < 7; c++)
            {
                header.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                header.Children.Add(dayLabel(dayPrefix[c], c));
            }

            header.SetValue(Grid.RowProperty, 1);

            return header;
        }

        private static Control ToHeader(this DateTime firstDate)
        {
            var label = new Label() { HorizontalContentAlignment = HorizontalAlignment.Center, Content = firstDate.ToString("y") };

            label.SetValue(Grid.RowProperty, 0);

            return label;
        }

        private static UIElement DateBox(this String content, String toolTip = null)
            => new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = content,
                TextAlignment = TextAlignment.Center,
                ToolTip = toolTip,
                Height = 20,
                Width = 20
            };

        private static UIElement ToView(this AvailableDay availableDay)
        {
            var border = new Border() { BorderThickness = new Thickness(1) };
            border.SetValue(Grid.RowProperty, availableDay.WeekIndex);
            border.SetValue(Grid.ColumnProperty, availableDay.DayIndex);
            border.BorderBrush = Brushes.DarkGray;

            border.Child = availableDay.Date.Day.ToString().DateBox();

            return border;
        }

        private static UIElement ToView(this OtherMonthDay otherMonthDay)
        {
            var label = new Label();

            label.SetValue(Grid.RowProperty, otherMonthDay.WeekIndex);
            label.SetValue(Grid.ColumnProperty, otherMonthDay.DayIndex);

            return label;
        }

        private static UIElement ToView(this UnavailableDay unavailableDay)
        {
            var border = new Border()
            { 
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.DarkGray,
                Background = unavailableDay.Category.Equals("holiday", StringComparison.OrdinalIgnoreCase)
                             ? Brushes.LightSalmon
                             : Brushes.LightSkyBlue,
                ToolTip = unavailableDay.Reason
            };

            border.SetValue(Grid.RowProperty, unavailableDay.WeekIndex);
            border.SetValue(Grid.ColumnProperty, unavailableDay.DayIndex);

            border.Child = unavailableDay.Date.Day.ToString().DateBox();

            return border;
        }

        private static UIElement ToView(this CalendarDay calendarDay)
        {
            return   calendarDay is AvailableDay availableDay     ? availableDay.ToView()
                   : calendarDay is OtherMonthDay otherMonthDay   ? otherMonthDay.ToView()
                   : calendarDay is UnavailableDay unavailableDay ? unavailableDay.ToView()
                   :                                                null;
        }

        private static UIElement ToBody(this IReadOnlyList<CalendarDay> days)
        {
            var container = new Grid();

            container.SetValue(Grid.RowProperty, 2);

            foreach (var row in Enumerable.Range(0, days.Count / 7))
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            for (Int32 c = 0; c < 7; c++)
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            foreach (var dayView in days.Select(d => d.ToView()))
                container.Children.Add(dayView);

            return container;
        }

        private static UIElement ToView(this CalendarMonth month)
        {
            var container = new Grid();

            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.Children.Add(month.FirstDate.ToHeader());
            container.Children.Add(DayHeader());
            container.Children.Add(month.Days.ToBody());

            return container;
        }

        public static Control ToView(this IEnumerable<CalendarMonth> calendarMonths)
        {
            var container = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Visible };
            var stack = new StackPanel() { Orientation = Orientation.Vertical };

            container.Content = stack;

            foreach (var monthView in calendarMonths.Select(m => m.ToView()))
                stack.Children.Add(monthView);

            return container;
        }

    }

}
