using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Nysa.Logics;

namespace Nysa.Json.Serialize
{

    public static class Write
    {
        public static String ToJson<T>(this T root, Func<JsonTextWriter, Unit> transform)
        {
            using (var sWriter = new StringWriter())
            using (var jWriter = new JsonTextWriter(sWriter))
            {
                transform(jWriter);

                return sWriter.ToString();
            }
        }

        public static Func<JsonTextWriter, Unit> WriteObject<T>(this T source, Func<JsonTextWriter, T, Unit> writeProperties)
            => w =>
            {
                w.WriteStartObject();
                writeProperties(w, source);
                w.WriteEndObject();

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteValue<T>(this T source)
            where T : struct
            => w =>
            {
                w.WriteValue(source);

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteValue(this String value)
            => w =>
            {
                w.WriteValue(value);

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteArray<T>(this IEnumerable<T> sources, Func<JsonTextWriter, T, Unit> writeSource)
            => w =>
            {
                w.WriteStartArray();
                foreach (var source in sources)
                    writeSource(w, source);
                w.WriteEndArray();

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteProperty(this String name, Func<JsonTextWriter, Unit> valueWriter)
            => w =>
            {
                w.WritePropertyName(name);
                return valueWriter(w);
            };

        public static Func<JsonTextWriter, Unit> WriteValueProperty<T>(this String propertyName, T value)
            where T : struct
            => w =>
            {
                w.WritePropertyName(propertyName);
                w.WriteValue(value);

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteValueProperty(this String propertyName, String value)
            => w =>
            {
                w.WritePropertyName(propertyName);
                w.WriteValue(value);

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> WriteObjectProperty<T>(this String propertyName, T source, Func<JsonTextWriter, T, Unit> writeProperties)
            => w =>
            {
                w.WritePropertyName(propertyName);
                w.WriteStartObject();
                writeProperties(w, source);
                w.WriteEndObject();

                return Unit.Value;
            };

        public static Func<JsonTextWriter, Unit> Then(this Func<JsonTextWriter, Unit> first, Func<JsonTextWriter, Unit> second)
            => w => { first(w); return second(w); };
    }

}
