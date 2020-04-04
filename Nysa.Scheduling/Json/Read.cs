using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace Nysa.Json.Deserialize
{

    public static class Read
    {
        public static T FromJson<T>(this String json, Func<JsonTextReader, T> transform)
        {
            using (var sReader = new StringReader(json))
            using (var jReader = new JsonTextReader(sReader))
            {
                return transform(jReader);
            }
        }


        public static readonly Func<JsonTextReader, Boolean>  Boolean  = r => Convert.ToBoolean((Int64)r.Value);
        public static readonly Func<JsonTextReader, Byte>     Byte     = r => Convert.ToByte((Int64)r.Value);
        public static readonly Func<JsonTextReader, DateTime> DateTime = r => System.DateTime.Parse((String)r.Value);
        public static readonly Func<JsonTextReader, Double>   Double   = r => Convert.ToDouble((Int64)r.Value);
        public static readonly Func<JsonTextReader, Int16>    Int16    = r => Convert.ToInt16((Int64)r.Value);
        public static readonly Func<JsonTextReader, Int32>    Int32    = r => Convert.ToInt32((Int64)r.Value);
        public static readonly Func<JsonTextReader, Int64>    Int64    = r => (Int64)r.Value;
        public static readonly Func<JsonTextReader, Single>   Single   = r => Convert.ToSingle((Int64)r.Value);
        public static readonly Func<JsonTextReader, String>   String   = r => (String)r.Value;

        public static readonly Func<JsonTextReader, Boolean>  BooleanProperty  = Read.PropertyValue(Read.Boolean);
        public static readonly Func<JsonTextReader, Byte>     ByteProperty     = Read.PropertyValue(Read.Byte);
        public static readonly Func<JsonTextReader, DateTime> DateTimeProperty = Read.PropertyValue(Read.DateTime);
        public static readonly Func<JsonTextReader, Double>   DoubleProperty   = Read.PropertyValue(Read.Double);
        public static readonly Func<JsonTextReader, Int16>    Int16Property    = Read.PropertyValue(Read.Int16);
        public static readonly Func<JsonTextReader, Int32>    Int32Property    = Read.PropertyValue(Read.Int32);
        public static readonly Func<JsonTextReader, Int64>    Int64Property    = Read.PropertyValue(Read.Int64);
        public static readonly Func<JsonTextReader, Single>   SingleProperty   = Read.PropertyValue(Read.Single);
        public static readonly Func<JsonTextReader, String>   StringProperty   = Read.PropertyValue(Read.String);


        public static readonly Func<JsonTextReader, String> PropertyName
            = r => { r.Read(); return (String)r.Value; };

        public static Func<JsonTextReader, T> PropertyValue<T>(this Func<JsonTextReader, T> contentReader)
            => r => { Read.PropertyName(r); r.Read(); return contentReader(r); };

        private static T ArrayObject<T>(JsonTextReader reader, Func<JsonTextReader, T> constructReader)
        {
            // No read. Already at: JsonToken.StartObject

            var result = constructReader(reader);

            reader.Read(); // JsonToken.EndObject
            reader.Read(); // JsonToken.StartObject || JsonToken.EndArray

            return result;
        }

        public static Func<JsonTextReader, IReadOnlyList<T>> Array<T>(Func<JsonTextReader, T> itemConstructReader)
            where T : class
            => r =>
            {
                var list = new List<T>();

                r.Read(); // JsonToken.StartArray
                r.Read(); // JsonToken.StartObject || JsonToken.EndArray

                while (r.TokenType != JsonToken.EndArray)
                    list.Add(Read.ArrayObject(r, itemConstructReader));

                // No read. Already at: JsonToken.EndArray

                return list;
            };

        public static Func<JsonTextReader, T> Object<T>(Func<JsonTextReader, T> constructReader)
            => r =>
            {
                r.Read(); // JsonToken.StartObject)

                var result = constructReader(r);

                r.Read(); // JsonToken.EndObject)

                return result;
            };

        public static Func<JsonTextReader, T> PropertyObject<T>(Func<JsonTextReader, T> constructReader)
            where T : class
            => r => { Read.PropertyName(r); return Read.Object(constructReader)(r); };

        
    }

}
