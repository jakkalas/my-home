using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using Newtonsoft.Json;

namespace MyHomeApi.Infrastructure.Smarthome.Models
{
    public class Device
    {
        [JsonProperty("deviceid")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("params")]
        public Params Params { get; set; }

        [JsonProperty("extra")]
        public ExtraParent Extra { get; set; }

        public int ChannelCount => EweLinkConstants.DevicesChannelLengths[EweLinkConstants.DeviceTypeUuid[Extra.Extra.Uiid]];
    }

    public class Params
    {
        [JsonProperty("switches")]
        public Switch[] Switches { get; set; }

        [JsonProperty("switch")]
        public string SwitchStatus { get; set; }

        [JsonIgnore]
        public bool IsPoweredOn => SwitchStatus != "off";
    }

    public class Switch
    {
        [JsonProperty("switch")]
        public string SwitchStatus { get; set; }

        [JsonProperty("outlet")]
        public int Outlet { get; set; }

        [JsonIgnore]
        public bool IsPoweredOn => SwitchStatus != "off";
    }

    public class ExtraParent
    {
        [JsonProperty("extra")]
        public Extra Extra { get; set; }
    }

    public class Extra
    {
        [JsonProperty("uiid")]
        public int Uiid { get; set; }
    }
}
