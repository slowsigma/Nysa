using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace Nysa.Windows.Markup
{

    public class AttributeBindingExtension : MarkupExtension, IValueConverter
    {
        public AttributeBindingExtension() { }

        public AttributeBindingExtension(String path) { this.Path = path; }

        [ConstructorArgument(@"path")]
        public String?          Path            { get; set; }
        public Object?          Source          { get; set; }
        public RelativeSource?  RelativeSource  { get; set; }

        private String GetAttributePropertyName()
        {
            if (this.Path == null) return String.Empty;

            String[] items = this.Path.Split('.');

            return ((items.Length > 1) ? ((items.Length > 2) ? items[2] : items[1]) : String.Empty);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Binding binding             = new Binding();
            binding.Converter           = this;
            binding.ConverterParameter  = this.Path;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            binding.Mode                = BindingMode.OneWay;

            if (this.Source != null)
            {
                binding.Source = this.Source;
            }
            else if (this.RelativeSource != null)
            {
                binding.RelativeSource = this.RelativeSource;
            }

            return binding.ProvideValue(serviceProvider);
        }

        object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            String[] path = this.Path.Split('.');
            PropertyInfo? property = null;

            // resolve path down to the next to last value as the last value will actually be the name of an attribute of the next to last property
            for (Int32 i = 0; i < (path.Length - 1); i++)
            {
                if (value == null) return Binding.DoNothing;
                property = value.GetType().GetProperty(path[i], (BindingFlags.Public | BindingFlags.Instance));
                if (property == null) return Binding.DoNothing;
                value = property.GetValue(value, null);
            }

            Object? attribute = property.GetCustomAttributes(false).FirstOrDefault(n => (n.GetType().Name.StartsWith(path[path.Length - 1], StringComparison.InvariantCultureIgnoreCase)));

            if (attribute == null) return Binding.DoNothing;

            PropertyInfo? attributeProperty = attribute.GetType().GetProperty(this.GetAttributePropertyName());

            if (attributeProperty == null) return Binding.DoNothing;

            return (String)attributeProperty.GetValue(attribute, null);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
