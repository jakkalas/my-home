using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink
{
    public class EweLinkService : ISmartHomeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EweLinkService(HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration["EweLink:Url"]);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["EweLink:Token"]);
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var response = await _httpClient.GetStringAsync($"user/device?lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            return JsonConvert.DeserializeObject<DevicesResult>(response).DeviceList;
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            var response = await _httpClient.GetStringAsync($"user/device/{deviceId}?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            return JsonConvert.DeserializeObject<Device>(response);
        }

        public async Task<bool> GetIsDevicePowerOn(string deviceId, int? channel)
        {
            var response = await _httpClient.GetStringAsync($"user/device/status?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            var device = JsonConvert.DeserializeObject<Device>(response);
            if (device.Params.Switches.Count() == 0)
            {
                return device.Params.IsPoweredOn;
            }
            return device.Params.Switches[channel ?? 0].IsPoweredOn;
        }

        public async Task<bool> ToggleDeviceAsync(string deviceId, int? channel)
        {
            dynamic request = new
            {
                appid = _configuration["EweLink:AppId"],
                deviceid = deviceId,
                @params = new
                {
                    switches = new List<dynamic>()
                }
            };
            var device = await GetDeviceAsync(deviceId);

            if (device.Params.Switches.Count() == 0)
            {
                request.@params = new
                {
                    @switch = device.Params.IsPoweredOn ? "off" : "on"
                };
            } 
            else
            {
                var index = 0;
                foreach (var switches in device.Params.Switches)
                {
                    if (index == channel)
                    {
                        request.@params.switches.Add(new
                        {
                            @switch = switches.IsPoweredOn ? "off" : "on",
                            outlet = switches.Outlet
                        });
                    }
                    index++;
                }                
            }
            var response = await _httpClient.PostAsync("user/device/status", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
    }
}
