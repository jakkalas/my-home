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
    }

    public class Params
    {
        [JsonProperty("switches")]
        public Switch[] Switches { get; set; }
    }
}
