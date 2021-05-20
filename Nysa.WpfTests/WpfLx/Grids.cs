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

    public static class Grids
    {
        private static readonly String ProgramError = "Program Error";

        public static GridSizedItem Star<T>(this T @this, Double ratio = 1.0, String? shareGroup = null)
            where T : UIElement
            => new GridSizedStar(@this, ratio, shareGroup);

        public static GridSizedItem Auto<T>(this T @this, String? shareGroup = null)
            where T : UIElement
            => new GridSizedAuto(@this, shareGroup);

        public static GridLength Star(this Int32  ratio) => new GridLength(ratio, GridUnitType.Star);
        public static GridLength Star(this Double ratio) => new GridLength(ratio, GridUnitType.Star);
        
        public static GridLength ToLength(this GridSizedItem @this)
            =>   @this is GridSizedAuto      ? GridLength.Auto
               : @this is GridSizedStar star ? star.Ratio.Star()
               :                               throw new Exception(ProgramError);

        public static T GridSet<T>(this T element, Int32 row, Int32 column)
            where T : UIElement
            => element.Affected(e => { e.SetValue(Grid.RowProperty, row); e.SetValue(Grid.ColumnProperty, column); });

        public static RowDefinition ToRowDef(this GridSizedItem @this)
            => (new RowDefinition() { Height = @this.ToLength() })
               .Affected(d =>
               {
                   if (!String.IsNullOrWhiteSpace(@this.ShareGroup))
                       d.SetValue(RowDefinition.SharedSizeGroupProperty, @this.ShareGroup);
               });

        public static ColumnDefinition ToColumnDef(this GridSizedItem @this)
            => (new ColumnDefinition() { Width = @this.ToLength() })
               .Affected(d =>
               {
                   if (!String.IsNullOrWhiteSpace(@this.ShareGroup))
                       d.SetValue(ColumnDefinition.SharedSizeGroupProperty, @this.ShareGroup);
               });

        // public static Grid WithRows(this Grid @this, params GridLength[] rowHeights)
        //     => rowHeights.Affect(h => @this.RowDefinitions.Add(new RowDefinition() { Height = h }));
        // public static Grid WithRows(this Grid @this, IEnumerable<GridLength> rowHeights)
        //     => rowHeights.Affect(h => @this.RowDefinitions.Add(new RowDefinition() { Height = h }));

        // public static Grid WithColumns(this Grid @this, params GridLength[] columnWidths)
        //     => columnWidths.Affect(w => @this.ColumnDefinitions.Add(new ColumnDefinition() { Width = w }));
        // public static Grid WithColumns(this Grid @this, params IEnumerable<GridLength> columnWidths)
        //     => columnWidths.Affect(w => @this.ColumnDefinitions.Add(new ColumnDefinition() { Width = w }));

        public static Grid RowGrid(params GridSizedItem[] rowItems)
            => (new Grid()).Affected(g =>
                {
                    rowItems.Affect((e, i) =>
                    {
                        g.RowDefinitions.Add(e.ToRowDef());
                        e.Visual.GridSet(i, 0);
                        g.Children.Add(e.Visual);
                    });
                });

        public static Grid RowGrid(IEnumerable<GridSizedItem> rowItems)
            => (new Grid()).Affected(g =>
                {
                    rowItems.Affect((e, i) =>
                    {
                        g.RowDefinitions.Add(e.ToRowDef());
                        e.Visual.GridSet(i, 0);
                        g.Children.Add(e.Visual);
                    });
                });

        public static Grid ColumnGrid(params GridSizedItem[] columnItems)
            => (new Grid()).Affected(g =>
                {
                    columnItems.Affect((e, i) =>
                    {
                        g.ColumnDefinitions.Add(e.ToColumnDef());
                        e.Visual.GridSet(0, i);
                        g.Children.Add(e.Visual);
                    });
                });

        public static Grid ColumnGrid(IEnumerable<GridSizedItem> columnItems)
            => (new Grid()).Affected(g =>
                {
                    columnItems.Affect((e, i) =>
                    {
                        g.ColumnDefinitions.Add(e.ToColumnDef());
                        e.Visual.GridSet(0, i);
                        g.Children.Add(e.Visual);
                    });
                });

        public static GridSizedItem RowSeparator()
            => (new Border()).Affected(b =>
            {
                b.BorderThickness = new Thickness(0, 0, 0, 5);
                b.BorderBrush = Colors.WhiteSmoke.AsBrush();
                b.Child = new DockPanel();
                b.Margin = new Thickness(0, 12, 0, 12);
            }).Auto();

        public static GridSizedItem ColumnSeparator()
            => (new Border()).Affected(b =>
            {
                b.BorderThickness = new Thickness(0, 0, 5, 0);
                b.BorderBrush = Colors.WhiteSmoke.AsBrush();
                b.Child = new DockPanel();
                b.Margin = new Thickness(12, 0, 12, 0);
            }).Auto();
    }

}