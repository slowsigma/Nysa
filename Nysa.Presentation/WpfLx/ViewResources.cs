using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Nysa.WpfLx
{

    public static class ViewResources
    {
        private static readonly Uri ViewResourcesUri;

        public static readonly ResourceDictionary Items;

        public static readonly Style BasicTextBoxStyle;
        public static readonly Style WindowContentBlockStyle;
        public static readonly Style WindowContentButtonStyle;
        public static readonly Style WindowContentAreaPanelStyle;
        public static readonly Style ComboBoxFlatStyle;
        public static readonly Style DataHeaderLabelStyle;
        public static readonly SolidColorBrush NormalBorderBrush;
        public static readonly SolidColorBrush InvalidBorderBrush;

        static ViewResources()
        {
            ViewResourcesUri = new Uri("/Nysa.Presentation;component/WpfLx/BaseResources.xaml", UriKind.Relative);
            Items = (ResourceDictionary)Application.LoadComponent(ViewResourcesUri);

            BasicTextBoxStyle = (Style)ViewResources.Items["BasicTextBox"];
            WindowContentBlockStyle = (Style)ViewResources.Items["WindowContentBlock"];
            WindowContentButtonStyle = (Style)ViewResources.Items["WindowContentButton"];
            WindowContentAreaPanelStyle = (Style)ViewResources.Items["WindowContentAreaPanel"];
            ComboBoxFlatStyle = (Style)ViewResources.Items["ComboBoxFlatStyle"];
            DataHeaderLabelStyle = (Style)ViewResources.Items["DataHeaderLabel"];
            NormalBorderBrush = (SolidColorBrush)ViewResources.Items["NormalBorderBrush"];
            InvalidBorderBrush = (SolidColorBrush)ViewResources.Items["InvalidBorderBrush"];
        }

    }

}
