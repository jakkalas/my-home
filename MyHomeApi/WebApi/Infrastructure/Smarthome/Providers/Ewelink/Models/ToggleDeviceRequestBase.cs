using Newtonsoft.Json;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models
{
	public class ToggleDeviceRequestBase
	{
		[JsonProperty("appid")]
		public string AppId { get; set; }

		[JsonProperty("deviceid")]
		public string DeviceId { get; set; }
	}
}
