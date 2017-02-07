using Newtonsoft.Json;
using System.Globalization;

namespace MyCoreLib.Data.Entity
{
    public abstract class ExtendableEntity : IExtendableEntity
    {
        private static JsonSerializerSettings s_jsonSerializerSettings = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Converts all the attributes to a string.
        /// </summary>
        public string GetAttributesAsString()
        {
            //IExtendedAttributesFactory factory = ExtendedAttributesCompiler.GetAttributesFactory(GetType());
            //IExtendedAttributes attrs = factory.CreateAttributes(this);
            //return JsonConvert.SerializeObject(attrs, s_jsonSerializerSettings);
            return null;
        }

        /// <summary>
        /// Sets the attributes of the <see cref="ExtendableEntity"/> object.
        /// </summary>
        public void SetAttributes(string json)
        {
            //IExtendedAttributesFactory factory = ExtendedAttributesCompiler.GetAttributesFactory(GetType());
            //IExtendedAttributes attrs = factory.DeserializeJson(json, s_jsonSerializerSettings);
            //if (attrs != null)
            //{
            //    factory.FillEntity(this, attrs);
            //}
        }
    }
}
