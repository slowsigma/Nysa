using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

using Nysa.Logics;

namespace Nysa.Text
{

    public static class TextExtensions
    {
        public static String? NullIfEmpty(this String value)
            => String.IsNullOrWhiteSpace(value) ? null : value.Trim();

        public static Boolean DataEquals(this String value, String other)
            => value == null ? (other == null) : value.Equals(other, StringComparison.OrdinalIgnoreCase);
        public static Boolean DataStartsWith(this String value, String starting)
            => value == null ? false : value.StartsWith(starting, StringComparison.OrdinalIgnoreCase);
        public static Boolean DataEndsWith(this String value, String ending)
            => value == null ? false : value.EndsWith(ending, StringComparison.OrdinalIgnoreCase);
        public static Int32 DataIndexOf(this String value, String find, Int32 startIndex = 0)
            => value == null ? -1 : value.IndexOf(find, startIndex, StringComparison.OrdinalIgnoreCase);


        private delegate Boolean TryParse<T>(String value, out T? result);
        //private delegate Boolean TryParse<T>(String value, out T? result) where T : class?;

        private static T? ParseValue<T>(this String value, TryParse<T> parse)
            where T : struct
        {
            T result;

            return (parse(value, out result) ? result : (T?)null);
        }

        private static T? ParseObject<T>(this String value, TryParse<T> parse)
            where T : class
        {
            T? result;

            return (parse(value, out result) ? result : null);
        }

        // parsing from string
        public static Int64? ParseInt64(this String value) => value.ParseValue<Int64>(Int64.TryParse);
        public static Int32? ParseInt32(this String value) => value.ParseValue<Int32>(Int32.TryParse);
        public static Int16? ParseInt16(this String value) => value.ParseValue<Int16>(Int16.TryParse);
        public static UInt64? ParseUInt64(this String value) => value.ParseValue<UInt64>(UInt64.TryParse);
        public static UInt32? ParseUInt32(this String value) => value.ParseValue<UInt32>(UInt32.TryParse);
        public static UInt16? ParseUInt16(this String value) => value.ParseValue<UInt16>(UInt16.TryParse);
        public static SByte? ParseSByte(this String value) => value.ParseValue<SByte>(SByte.TryParse);
        public static Single? ParseSingle(this String value) => value.ParseValue<Single>(Single.TryParse);
        public static DateTime? ParseDateTime(this String value) => value.ParseValue<DateTime>(DateTime.TryParse);
        public static TimeSpan? ParseTimeSpan(this String value) => value.ParseValue<TimeSpan>(TimeSpan.TryParse);
        public static DateTimeOffset? ParseDateTimeOffset(this String value) => value.ParseValue<DateTimeOffset>(DateTimeOffset.TryParse);
        public static Guid? ParseGuid(this String value) => value.ParseValue<Guid>(Guid.TryParse);
        public static Decimal? ParseDecimal(this String value) => value.ParseValue<Decimal>(Decimal.TryParse);
        public static Double? ParseDouble(this String value) => value.ParseValue<Double>(Double.TryParse);
        public static Byte? ParseByte(this String value) => value.ParseValue<Byte>(Byte.TryParse);

        public static Version? ParseVersion(this String value) => value.ParseObject<Version>(Version.TryParse);

        public static Boolean? ParseBoolean(this String value)
        {
            if (String.IsNullOrWhiteSpace(value)) return (Boolean?)null;

            Decimal? number = value.ParseDecimal();

            if (number.HasValue) return number.Value != 0;

            switch (value.ToLower())
            {
                case @"no":
                case @"false":
                    return false;
                case @"yes":
                case @"true":
                    return true;
                default:
                    return (Boolean?)null;
            }
        }


        private static Suspect<T?> ToValue<T>(this String value, TryParse<T> parse)
            where T : struct
        {
            T result;

            return   String.IsNullOrWhiteSpace(value) ? ((T?)null).Confirmed()
                   : parse(value, out result)         ? ((T?)result).Confirmed()
                   :                                    (new InvalidDataException()).Failed<T?>();
        }

        private static Suspect<Option<T>> ToObject<T>(this String value, TryParse<T> parse)
            where T : class
        {
            T? result;

            return   String.IsNullOrWhiteSpace(value) ? Option<T>.None.Confirmed()
                   : parse(value, out result)         ? result != null ? result.Some<T>().Confirmed() : Option<T>.None.Confirmed()
                   :                                    (new InvalidDataException()).Failed<Option<T>>();
        }

        public static Suspect<Int64?> ToInt64(this String value) => value.ToValue<Int64>(Int64.TryParse);
        public static Suspect<Int32?> ToInt32(this String value) => value.ToValue<Int32>(Int32.TryParse);
        public static Suspect<Int16?> ToInt16(this String value) => value.ToValue<Int16>(Int16.TryParse);
        public static Suspect<Byte?> ToByte(this String value) => value.ToValue<Byte>(Byte.TryParse);
        public static Suspect<Single?> ToSingle(this String value) => value.ToValue<Single>(Single.TryParse);
        public static Suspect<DateTime?> ToDateTime(this String value) => value.ToValue<DateTime>(DateTime.TryParse);
        public static Suspect<TimeSpan?> ToTimeSpan(this String value) => value.ToValue<TimeSpan>(TimeSpan.TryParse);
        public static Suspect<DateTimeOffset?> ToDateTimeOffset(this String value) => value.ToValue<DateTimeOffset>(DateTimeOffset.TryParse);
        public static Suspect<Guid?> ToGuid(this String value) => value.ToValue<Guid>(Guid.TryParse);
        public static Suspect<Decimal?> ToDecimal(this String value) => value.ToValue<Decimal>(Decimal.TryParse);
        public static Suspect<Double?> ToDouble(this String value) => value.ToValue<Double>(Double.TryParse);

        public static Suspect<Option<Version>> ToVersion(this String value) => value.ToObject<Version>(Version.TryParse);

        public static Suspect<Boolean?> ToBoolean(this String value)
        {
            if (String.IsNullOrWhiteSpace(value)) return ((Boolean?)null).Confirmed();

            // NOTE: Intentionally changed the order of this from ParseBoolean
            //       assuming true and false are more often used than a 0 or 1.
            switch (value.ToLower())
            {
                case @"no":
                case @"false":
                    return ((Boolean?)false).Confirmed();
                case @"yes":
                case @"true":
                    return ((Boolean?)true).Confirmed();
                default:
                    break;
            }

            Decimal? number = value.ParseDecimal();

            if (number.HasValue) return ((Boolean?)(number.Value != 0)).Confirmed();

            return (new InvalidDataException()).Failed<Boolean?>();
        }

        public static Suspect<Option<Byte[]>> ToBytes(this String hexString)
        {
            Func<Option<Byte[]>> convert = () =>
            {
                hexString = hexString.DataStartsWith("0x") ? hexString.Substring(2) : hexString;

                if (String.IsNullOrWhiteSpace(hexString)) return Option<Byte[]>.None;

                var value = Enumerable.Range(0, hexString.Length / 2)
                                      .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
                                      .ToArray();

                return value.Some<Byte[]>();
            };

            return convert.Try().Eval();
        }


        public static String ToHexString(this Byte[] bytes)
            => bytes.Aggregate((Builder: (new StringBuilder((bytes.Length * 2) + 2)).Append("0x"),
                                HexTable: "0123456789ABCDEF"),
                               (c, b) => c.Affected(d => d.Builder
                                                          .Append(d.HexTable[(Int32)b >> 4])
                                                          .Append(d.HexTable[(Int32)(b & 0x0F)])),
                               f => f.Builder.ToString());

        public static String ToHexByteList(this Byte[] bytes)
            => bytes.Aggregate((Builder: new StringBuilder((bytes.Length * 6)),
                                HexTable: "0123456789ABCDEF"),
                               (c, b) => c.Affected(d => d.Builder
                                                          .Append("0x")
                                                          .Append(d.HexTable[(Int32)b >> 4])
                                                          .Append(d.HexTable[(Int32)(b & 0x0F)])
                                                          .Append(", ")),
                               h => h.Builder.ToString().Substring(0, h.Builder.Length - 2));


        public static String? TruncateTo(this String? value, Int32 maxLength)
            => (value != null && value.Length > maxLength)
               ? value.Substring(0, maxLength)
               : value;

        public static Byte[] Compressed(this Byte[] bytes)
        {
            using (var outpt = new MemoryStream())
            {
                using (var zipit = new GZipStream(outpt, CompressionMode.Compress, true))
                {
                    zipit.Write(bytes, 0, bytes.Length);
                }

                outpt.Position = 0;

                return outpt.ToArray();
            }
        }

        public static String CompressedToBase64(this String data)
        {
            using (var outpt = new MemoryStream())
            {
                using (var zipit = new GZipStream(outpt, CompressionMode.Compress, true))
                {
                    var bytes = Encoding.UTF8.GetBytes(data);

                    zipit.Write(bytes, 0, bytes.Length);
                }

                outpt.Position = 0;

                return Convert.ToBase64String(outpt.ToArray());
            }
        }

        public static Byte[] Uncompressed(this Byte[] bytes)
        {
            using (var outpt = new MemoryStream())
            {
                outpt.Write(bytes, 0, bytes.Length);

                outpt.Position = 0;

                using (var zipit = new GZipStream(outpt, CompressionMode.Decompress))
                using (var final = new MemoryStream())
                {
                    zipit.CopyTo(final);

                    return final.ToArray();
                }
            }
        }

        public static String UncompressFromBase64(this String compressedBase64)
        {
            using (var outpt = new MemoryStream())
            {
                var bytes = Convert.FromBase64String(compressedBase64);

                outpt.Write(bytes, 0, bytes.Length);

                outpt.Position = 0;

                using (var zipit = new GZipStream(outpt, CompressionMode.Decompress))
                using (var final = new MemoryStream())
                {
                    zipit.CopyTo(final);

                    return Encoding.UTF8.GetString(final.ToArray());
                }
            }
        }

    }

}
