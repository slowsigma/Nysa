using System;
using System.Windows;

namespace Nysa.WpfLx
{

    public record GridSizedStar(UIElement Visual, Double Ratio = 1.0, String? ShareGroup = null) : GridSizedItem(Visual, ShareGroup) { }

}