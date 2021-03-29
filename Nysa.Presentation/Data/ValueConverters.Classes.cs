using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nysa.Windows.Data
{

    public static partial class ValueConverters
    {

        private static class Classes
        {

            public class SafeStructString<TSource> : IValueConverter
                where TSource : struct
            {
                static SafeStructString()
                {
                    Type thisType = typeof(SafeStructString<TSource>);

                    TypeDescriptor.AddAttributes(thisType, new ValueConversionAttribute(typeof(TSource), typeof(String)));
                }

                // instance members
                private Func<TSource?, Type, Object, CultureInfo, String>   _Convert;
                private Object                                              _ConvertFailureValue;
                private Func<String, Type, Object, CultureInfo, TSource?>   _ConvertBack;
                private Object                                              _ConvertBackFailureValue;

                public SafeStructString(Func<TSource, Type, Object, CultureInfo, String> convert, Object convertFailureValue, Func<String, Type, Object, CultureInfo, TSource?> convertBack, Object convertBackFailureValue)
                {
                    this._Convert                   = (convert == null) ? (Func<TSource?, Type, Object, CultureInfo, String>)null : (s, y, p, c) => s.HasValue ? convert(s.Value, y, p, c) : null;
                    this._ConvertFailureValue       = convertFailureValue;
                    this._ConvertBack               = convertBack;
                    this._ConvertBackFailureValue   = convertBackFailureValue;
                }

                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._Convert   == null                         ) throw new NotImplementedException();
                    if (value           == Binding.DoNothing            ) return value;
                    if (value           == DependencyProperty.UnsetValue) return value;

                    TSource? basis =  (value is TSource)  ? (TSource)value
                                    : (value is TSource?) ? (TSource?)value
                                    :                       (TSource?)null;

                    Object result = this._Convert(basis, targetType, parameter, culture);

                    return (result == null) ? this._ConvertFailureValue : result;
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._ConvertBack   == null                         ) throw new NotImplementedException();
                    if (value               == Binding.DoNothing            ) return value;
                    if (value               == DependencyProperty.UnsetValue) return value;

                    TSource? basis = this._ConvertBack(value as String, targetType, parameter, culture);

                    return basis.HasValue ? basis.Value : this._ConvertBackFailureValue;
                }
            
            }

            public class SafeStringStruct<TTarget> : IValueConverter
                where TTarget : struct
            {
                static SafeStringStruct()
                {
                    Type thisType = typeof(SafeStructString<TTarget>);

                    TypeDescriptor.AddAttributes(thisType, new ValueConversionAttribute(typeof(String), typeof(TTarget)));
                }

                // instance members
                private Func<String, Type, Object, CultureInfo, TTarget?>   _Convert;
                private Object                                              _ConvertFailureValue;
                private Func<TTarget?, Type, Object, CultureInfo, String>   _ConvertBack;
                private Object                                              _ConvertBackFailureValue;

                public SafeStringStruct(Func<String, Type, Object, CultureInfo, TTarget?> convert, Object convertFailureValue, Func<TTarget, Type, Object, CultureInfo, String> convertBack, Object convertBackFailureValue)
                {
                    this._Convert                   = convert;
                    this._ConvertFailureValue       = convertFailureValue;
                    this._ConvertBack               = (convertBack == null) ? (Func<TTarget?, Type, Object, CultureInfo, String>)null : (t, y, p, c) => t.HasValue ? convertBack(t.Value, y, p, c) : null;
                    this._ConvertBackFailureValue   = convertBackFailureValue;
                }

                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._Convert   == null                         ) throw new NotImplementedException();
                    if (value           == Binding.DoNothing            ) return value;
                    if (value           == DependencyProperty.UnsetValue) return value;

                    TTarget? basis = this._Convert(value as String, targetType, parameter, culture);

                    return basis.HasValue ? basis.Value : this._ConvertFailureValue;
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._ConvertBack   == null                         ) throw new NotImplementedException();
                    if (value               == Binding.DoNothing            ) return value;
                    if (value               == DependencyProperty.UnsetValue) return value;

                    TTarget? basis =  (value is TTarget)  ? (TTarget)value
                                    : (value is TTarget?) ? (TTarget?)value
                                    :                       (TTarget?)null;

                    Object result = this._ConvertBack(basis, targetType, parameter, culture);

                    return (result == null) ? this._ConvertBackFailureValue : result;
                }

            }

            public class ValueConverter<TSource, TTarget> : IValueConverter
            {
                static ValueConverter()
                {
                    Type thisType = typeof(ValueConverter<TSource, TTarget>);

                    TypeDescriptor.AddAttributes(thisType, new ValueConversionAttribute(typeof(TSource), typeof(TTarget)));
                }

                // instance members
                private Func<TSource, Type, Object, CultureInfo, TTarget> _Convert;
                private Func<TTarget, Type, Object, CultureInfo, TSource> _ConvertBack;

                public ValueConverter(Func<TSource, Type, Object, CultureInfo, TTarget> convert, Func<TTarget, Type, Object, CultureInfo, TSource> convertBack)
                {
                    this._Convert       = convert;
                    this._ConvertBack   = convertBack;
                }

                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._Convert == null) throw new NotImplementedException();
                    if (value == Binding.DoNothing) return value;
                    if (value == DependencyProperty.UnsetValue) return value;

                    try
                    {
                        return this._Convert((TSource)value, targetType, parameter, culture);
                    }
                    catch
                    {
                        return default(TSource);
                    }
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    if (this._ConvertBack == null) throw new NotImplementedException();
                    if (value == Binding.DoNothing) return value;
                    if (value == DependencyProperty.UnsetValue) return value;

                    try
                    {
                        return this._ConvertBack((TTarget)value, targetType, parameter, culture);
                    }
                    catch
                    {
                        return default(TSource);
                    }
                }

            }

        }

    }

}
