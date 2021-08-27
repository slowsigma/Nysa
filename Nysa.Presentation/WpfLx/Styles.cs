using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public static class Styles
    {
        public static Style StyleTarget(this Type @this)
            => new Style(@this);

        public static T Styled<T>(this T element, Object style)
            where T : FrameworkElement
            => element.Affected(e => e.Style = (Style)style);

        public static Style WithFont(this Style @this, String? family = null, Int32? size = null, FontWeight? weight = null, FontStyle? style = null)
            => @this.Affected(s =>
            {
                if (!String.IsNullOrWhiteSpace(family))
                    s.Setters.Add(Control.FontFamilyProperty.AsSetter(new FontFamily(family)));
                if (size.HasValue)
                    s.Setters.Add(Control.FontSizeProperty.AsSetter(Convert.ToDouble(size.Value)));
                if (weight.HasValue)
                    s.Setters.Add(Control.FontWeightProperty.AsSetter(weight.Value));
                if (style.HasValue)
                    s.Setters.Add(Control.FontStyleProperty.AsSetter(style.Value));
            });
        public static Style WithMargin(this Style @this, Double left, Double top, Double right, Double bottom)
            => @this.Affected(s => s.Setters.Add(Control.MarginProperty.AsSetter(new Thickness(left, top, right, bottom))));
        public static Style WithPadding(this Style @this, Double left, Double top, Double right, Double bottom)
            => @this.Affected(s => s.Setters.Add(Control.PaddingProperty.AsSetter(new Thickness(left, top, right, bottom))));
        public static Style WithPadding(this Style @this, Double all)
            => @this.Affected(s => s.Setters.Add(Control.PaddingProperty.AsSetter(new Thickness(all, all, all, all))));
        public static Style WithBorderThickness(this Style @this, Double all)
            => @this.Affected(s => s.Setters.Add(Control.BorderThicknessProperty.AsSetter(new Thickness(all, all, all, all))));
        public static Style WithVisibility(this Style @this, object visibility)
            => @this.Affected(s => s.Setters.Add(Control.VisibilityProperty.AsSetter(visibility)));
        public static Style WithVisibility(this Style @this, Visibility visibility)
            => @this.Affected(s => s.Setters.Add(Control.VisibilityProperty.AsSetter(visibility)));
        public static Style WithForeground(this Style @this, SolidColorBrush brush)
            => @this.Affected(s => s.Setters.Add(Control.ForegroundProperty.AsSetter(brush)));
        public static Style WithBackground(this Style @this, SolidColorBrush brush)
            => @this.Affected(s => s.Setters.Add(Control.BackgroundProperty.AsSetter(brush)));
        public static Style WithSetter(this Style @this, Setter setter)
            => @this.Affected(s => s.Setters.Add(setter));
        public static Style WithSetters(this Style @this, params Setter[] setters)
            => @this.Affected(y => setters.Affect(s => { y.Setters.Add(s); }));

    }

}