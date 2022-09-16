using MyHomeApi.Entities;
using MyHomeApi.Infrastructure.Models;
using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Channels;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink
{
    public class EweLinkService : ISmartHomeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EweLinkService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration["EweLink:Url"]);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["EweLink:Token"]);
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var response = await _httpClient.GetAsync($"user/device?lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            var responseContent = await response.Content.ReadAsStringAsync();
            ProcessResponse(response, responseContent);
            return JsonConvert.DeserializeObject<DevicesResult>(responseContent).DeviceList;
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            var response = await _httpClient.GetAsync($"user/device/{deviceId}?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            var responseContent = await response.Content.ReadAsStringAsync();
            ProcessResponse(response, responseContent);
            return JsonConvert.DeserializeObject<Device>(responseContent);
        }

        public async Task<bool> GetIsDevicePowerOn(
            string deviceId, 
            int? channel)
        {
            var response = await _httpClient.GetAsync($"user/device/status?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            var responseContent = await response.Content.ReadAsStringAsync();
            ProcessResponse(response, responseContent);
            var device = JsonConvert.DeserializeObject<Device>(responseContent);
            if (device.Params.Switches.Count() == 0)
            {
                return device.Params.IsPoweredOn;
            }
            return device.Params.Switches[channel ?? 0].IsPoweredOn;
        }

        public async Task<bool> ToggleDeviceAsync(
            string deviceId, 
            int? channel)
        {
            var device = await GetDeviceAsync(deviceId);

            if (device.Params.Switches.Count() == 0)
            {
                return await ToggleSingleSwitchDeviceAsync(
                    device, 
                    deviceId);
            }

            return await ToggleMultipleSwitchDeviceAsync(
                device,
                deviceId,
                channel);
		}

        private async Task<bool> ToggleSingleSwitchDeviceAsync(
            Device device,
			string deviceId)
        {
            var request = new ToggleSingleSwitchDeviceRequest(
                _configuration["EweLink:AppId"],
                deviceId,
                device);

			var response = await _httpClient.PostAsync(
                "user/device/status", 
                new StringContent(JsonConvert.SerializeObject(request), 
                Encoding.UTF8, 
                "application/json"));
			return response.IsSuccessStatusCode;
		}

		private async Task<bool> ToggleMultipleSwitchDeviceAsync(
			Device device,
			string deviceId,
			int? channel)
		{
			var request = new ToggleMultipleSwitchDeviceRequest(
                _configuration["EweLink:AppId"],
				deviceId);
            request.SetSwitchParams(
                device, 
                channel);

			var response = await _httpClient.PostAsync(
                "user/device/status", 
                new StringContent(JsonConvert.SerializeObject(request), 
                Encoding.UTF8, 
                "application/json"));
			return response.IsSuccessStatusCode;
		}

		private void ProcessResponse(
            HttpResponseMessage response, 
            string responseContent) 
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                HttpErrorMessage message;
                try
                {
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        message = new HttpErrorMessage
                        {
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        message = JsonConvert.DeserializeObject<HttpErrorMessage>(responseContent);
                    }
                }
                catch (Exception)
                {
                    message = new HttpErrorMessage
                    {
                        Message = $"Error parsing exception response from server. Raw Response: {responseContent}"
                    };
                }

                throw new ApiException(response.StatusCode, message, response);
            }
        }
    }
}
