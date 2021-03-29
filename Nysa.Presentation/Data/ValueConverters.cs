using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Nysa.Text;

using Nysa.Windows.Markup;

namespace Nysa.Windows.Data
{

    public static partial class ValueConverters
    {
        // constructors
        public static IValueConverter New<TSource, TTarget>(Func<TSource, TTarget> convert) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s), null); }
        public static IValueConverter New<TSource, TTarget>(Func<TSource, TTarget> convert, Func<TTarget, TSource> convertBack) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s), (t, y, p, c) => convertBack(t)); }
        public static IValueConverter New<TSource, TTarget>(Func<TSource, Object, TTarget> convert, Func<TTarget, Object, TSource> convertBack) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s, p), (t, y, p, c) => convertBack(t, p)); }
        public static IValueConverter New<TSource, TTarget>(Func<TSource, Object, CultureInfo, TTarget> convert, Func<TTarget, Object, CultureInfo, TSource> convertBack) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s, p, c), (t, y, p, c) => convertBack(t, p, c)); }
        public static IValueConverter New<TSource, TTarget>(Func<TSource, Type, Object, CultureInfo, TTarget> convert, Func<TTarget, Type, Object, CultureInfo, TSource> convertBack) { return new Classes.ValueConverter<TSource, TTarget>(convert, convertBack); }

        public static IValueConverter New<TSource, TTarget, TParameter>(Func<TSource, TParameter, TTarget> convert) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s, (TParameter)p), null); }
        public static IValueConverter New<TSource, TTarget, TParameter>(Func<TSource, TParameter, TTarget> convert, Func<TTarget, TParameter, TSource> convertBack) { return new Classes.ValueConverter<TSource, TTarget>((s, y, p, c) => convert(s, (TParameter)p), (t, y, p, c) => convertBack(t, (TParameter)p)); }

        public static IValueConverter New(Func<Object, Type, Object, CultureInfo, Object> convert, Func<Object, Type, Object, CultureInfo, Object> convertBack) { return new Classes.ValueConverter<Object, Object>(convert, convertBack); }

        public static IValueConverter New<TSource>(Func<TSource, Type, Object, CultureInfo, String> convert, Object convertFailureValue, Func<String, Type, Object, CultureInfo, TSource?> convertBack, Object convertBackFailureValue)
            where TSource : struct
        { 
            return new Classes.SafeStructString<TSource>(convert, convertFailureValue, convertBack, convertBackFailureValue);
        }

        public static IValueConverter New<TTarget>(Func<String, Type, Object, CultureInfo, TTarget?> convert, Object convertFailureValue, Func<TTarget, Type, Object, CultureInfo, String> convertBack, Object convertBackFailureValue)
            where TTarget : struct
        {
            return new Classes.SafeStringStruct<TTarget>(convert, convertFailureValue, convertBack, convertBackFailureValue);
        }


        public static IValueConverter ObjectNotNull             = New<Object, Boolean>(s => !(s == null));

        public static IValueConverter BooleanInverter           = New<Boolean, Boolean>(s => !s, t => !t);
        public static IValueConverter BooleanVisibleCollapsed   = New<Boolean, Visibility>(s => s ? Visibility.Visible : Visibility.Collapsed, t => t == Visibility.Visible);
        public static IValueConverter BooleanVisibleHidden      = New<Boolean, Visibility>(s => s ? Visibility.Visible : Visibility.Hidden, t => t == Visibility.Visible);


        public static IValueConverter BooleanStringOrNothing    = New<Boolean>(Convert, Binding.DoNothing, (t, y, p, c) => t.ParseBoolean(), Binding.DoNothing);
        public static IValueConverter DateTimeStringOrNothing   = New<DateTime>(Convert, Binding.DoNothing, (t, y, p, c) => t.ParseDateTime(), Binding.DoNothing);
        public static IValueConverter DecimalStringOrNothing    = New<Decimal>(Convert, Binding.DoNothing, (t, y, p, c) => t.ParseDecimal(), Binding.DoNothing);
        public static IValueConverter Int32StringOrNothing      = New<Decimal>(Convert, Binding.DoNothing, (t, y, p, c) => t.ParseInt32(), Binding.DoNothing);
        public static IValueConverter GuidStringOrNothing       = New<Guid>(Convert, Binding.DoNothing, (t, y, p, c) => t.ParseGuid(), Binding.DoNothing);

        public static IValueConverter NullableBooleanString     = New<Boolean>(Convert, String.Empty, (t, y, p, c) => t.ParseBoolean(), (Nullable<Boolean>)null);
        public static IValueConverter NullableDecimalString     = New<Decimal>(Convert, String.Empty, (t, y, p, c) => t.ParseDecimal(), (Nullable<Decimal>)null);
        public static IValueConverter NullableDateTimeString    = New<DateTime>(Convert, String.Empty, (t, y, p, c) => t.ParseDateTime(), (Nullable<DateTime>)null);
        public static IValueConverter NullableInt32String       = New<Int32>(Convert, String.Empty, (t, y, p, c) => t.ParseInt32(), (Nullable<Int32>)null);
        public static IValueConverter NullableGuidString        = New<Guid>(Convert, String.Empty, (t, y, p, c) => t.ParseGuid(), (Nullable<Guid>)null);

        public static IValueConverter StringNullableBoolean     = New<Boolean>((s, y, p, c) => s.ParseBoolean(), (Nullable<Boolean>)null, Convert, String.Empty);
        public static IValueConverter StringNullableDecimal     = New<Decimal>((s, y, p, c) => s.ParseDecimal(), (Nullable<Decimal>)null, Convert, String.Empty);
        public static IValueConverter StringNullableDateTime    = New<DateTime>((s, y, p, c) => s.ParseDateTime(), (Nullable<DateTime>)null, Convert, String.Empty);
        public static IValueConverter StringNullableInt32       = New<Int32>((s, y, p, c) => s.ParseInt32(), (Nullable<Int32>)null, Convert, String.Empty);
        public static IValueConverter StringNullableGuid        = New<Guid>((s, y, p, c) => s.ParseGuid(), (Nullable<Guid>)null, Convert, String.Empty);

        public static IValueConverter StringBooleanString       = new PipeExtension(StringNullableBoolean, NullableBooleanString);
        public static IValueConverter StringDecimalString       = new PipeExtension(StringNullableDecimal, NullableDecimalString);
        public static IValueConverter StringDateTimeString      = new PipeExtension(StringNullableDateTime, NullableDateTimeString);
        public static IValueConverter StringInt32String         = new PipeExtension(StringNullableInt32, NullableInt32String);
        public static IValueConverter StringGuidString          = new PipeExtension(StringNullableGuid, NullableGuidString);


        public static IValueConverter ObjectToArray             = New<Object, Object[]>(s => s == null ? new Object[] { } : new Object[] { s });
        public static IValueConverter ObjectOrNullToArray       = New<Object, Object[]>(s => new Object[] { s });

        private static String ParsedConvert<T>(T? value, Func<T, String> convert)
            where T : struct
        {
            return value.HasValue ? convert(value.Value) : String.Empty;
        }

        private static String Convert(Boolean value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter as String;

            if (parameterString != null)
            {
                switch (parameterString.ToLowerInvariant())
                {
                    case @"t/f":        return value ? @"T" : @"F";
                    case @"y/n":        return value ? @"Y" : @"N";
                    case @"true/false": return value ? @"True" : @"False";
                    case @"yes/no":     return value ? @"Yes" : @"No";
                    case @"1/0":        return value ? @"1" : "0";
                    case @"0/1":        return value ? @"1" : "0";
                    default:            return value ? @"True" : @"False";
                }
            }
            else
            {
                return value ? @"True" : @"False";
            }
        }

        private static String Convert(DateTime value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter as String;

            return (parameterString != null) ? value.ToString(parameterString) : value.ToString();
        }

        private static String Convert(Int32 value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter as String;

            return (parameterString != null) ? value.ToString(parameterString) : value.ToString();
        }

        private static String Convert(Decimal value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter as String;

            return (parameterString != null) ? value.ToString(parameterString) : value.ToString();
        }

        private static String Convert(Guid value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter as String;

            return (parameterString != null) ? value.ToString(parameterString) : value.ToString();
        }

    }

}
