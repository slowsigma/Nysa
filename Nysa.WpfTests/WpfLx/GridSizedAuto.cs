using System;
using System.Windows;

namespace Nysa.WpfLx
{

    public record GridSizedAuto(UIElement Visual, String? ShareGroup) : GridSizedItem(Visual, ShareGroup) { }

}