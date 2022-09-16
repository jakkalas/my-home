using MyHomeApi.Infrastructure.Smarthome.Models;
using Newtonsoft.Json;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models
{
	public class ToggleMultipleSwitchDeviceRequest : ToggleDeviceRequestBase
	{
		[JsonProperty("params")]
		public ToggleMultipleParams Params { get; set; }

		public ToggleMultipleSwitchDeviceRequest(
			string appId,
			string deviceId)
		{
			AppId = appId;
			DeviceId = deviceId;
			Params = new ToggleMultipleParams();
		}

		public void SetSwitchParams(
			Device device, 
			int? channel)
		{
			var index = 0;
			foreach (var switches in device.Params.Switches)
			{
				if (index == channel)
				{
					Params.Switches.Add(new ToggleMultipleSwitch
					{
						Switch = switches.GetToggleStatus,
						Outlet = switches.Outlet
					});
				}
				index++;
			}
		}
	}

	public class ToggleMultipleParams
	{
		[JsonProperty("switches")]
		public List<ToggleMultipleSwitch> Switches { get; set; }
	}

	public class ToggleMultipleSwitch
	{
		[JsonProperty("switch")]
		public string Switch { get; set; }

		[JsonProperty("outlet")]
		public int Outlet { get; set; }
	}
}
