
namespace MyCoreLib.BaseSMS.Netease
{
    using Newtonsoft.Json;

    public interface INetEaseSMSConfiguration
    {
        string SMSAppKey { get; }
        string SMSAppSecret { get; }
    }

    public class NetEaseSMSConfiguration : INetEaseSMSConfiguration
    {
        [JsonProperty("app_key")]
        public string SMSAppKey { get; set; }

        [JsonProperty("app_secret")]
        public string SMSAppSecret { get; set; }

        //public override string ToString()
        //{
        //    return this.ToJsonString(GlobalConfigurations.ConfigJsonSettings);
        //}

        /// <summary>
        /// Try to read and deserialize settings.
        /// </summary>
        //internal static INetEaseSMSConfiguration FromGlobalConfigs(GlobalConfigurations configs)
        //{
        //    string text = configs[GlobalConfigurationKey.NetEaseSMSConfigs];

        //    INetEaseSMSConfiguration settings = null;
        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        settings = text.FromJsonString<NetEaseSMSConfiguration>();
        //    }
        //    return settings ?? new NetEaseSMSConfiguration();
        //}
    }
}
