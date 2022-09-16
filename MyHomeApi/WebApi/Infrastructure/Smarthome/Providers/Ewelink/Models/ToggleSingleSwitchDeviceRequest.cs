using Newtonsoft.Json;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models
{
	public class ToggleSingleSwitchDeviceRequest : ToggleDeviceRequestBase
	{
		[JsonProperty("params")]
		public ToggleSingleParams Params { get; set; }

		public ToggleSingleSwitchDeviceRequest(
			string appId, 
			string deviceId,
			Smarthome.Models.Device device)
		{
			AppId = appId;
			DeviceId = deviceId;
			Params = new ToggleSingleParams
			{
				Switch = device.Params.GetToggleStatus
			};
		}
	}

	public class ToggleSingleParams
	{
		[JsonProperty("switch")]
		public string Switch { get; set; }
	}
}
