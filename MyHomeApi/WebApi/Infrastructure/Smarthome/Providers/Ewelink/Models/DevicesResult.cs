using MyHomeApi.Infrastructure.Smarthome.Models;
using Newtonsoft.Json;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models
{
    public class DevicesResult
    {
        [JsonProperty("devicelist")]
        public Device[] DeviceList { get; set; }
    }
}
