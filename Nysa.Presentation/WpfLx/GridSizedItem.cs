using System;
using System.Windows;

namespace Nysa.WpfLx
{

    public abstract record GridSizedItem(UIElement Visual, String? ShareGroup = null) { }

}