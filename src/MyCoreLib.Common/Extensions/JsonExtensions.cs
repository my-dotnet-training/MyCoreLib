
namespace MyCoreLib.Common.Extensions
{
    using System;
    using System.Globalization;
    using System.IO;
    using Newtonsoft.Json;

    public static class JsonExtensions
    {
        private static JsonSerializerSettings s_jsonSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            NullValueHandling = NullValueHandling.Ignore,

            /*
             * We must include default values. e.g. some members could have 0 at its value.
             * Ignoring zeroes could cause javascript errors.
             */
            DefaultValueHandling = DefaultValueHandling.Include
        };

        /// <summary>
        /// Convert an object to its JSON representation.
        /// </summary>
        public static string ToJsonString<T>(this T obj)
        {
            return ToJsonString(obj, s_jsonSettings);
        }

        /// <summary>
        /// Convert an object to its JSON representation.
        /// </summary>
        public static string ToJsonString<T>(this T obj, JsonSerializerSettings settings)
        {
            if (obj == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Convert an object to its JSON representation.
        /// </summary>
        public static T FromJsonString<T>(this Stream stream) where T : class
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var sr = new StreamReader(stream))
            {
                return FromJsonString<T>(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// Convert an object to its JSON representation.
        /// </summary>
        public static T FromJsonString<T>(this string s) where T : class
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (s == "null")
                return null;

            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
