using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using System.Net.Http.Headers;

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
            var json = await _httpClient.GetStringAsync($"user/device?lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            var result = await _httpClient.GetFromJsonAsync<DevicesResult>($"user/device?lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            return result.DeviceList;
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            return await _httpClient.GetFromJsonAsync<Device>($"user/device/{deviceId}?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
        }

        public async Task<bool> GetIsDevicePowerOn(string deviceId, int? channel)
        {
            var result = await _httpClient.GetFromJsonAsync<Device>($"user/device/status?deviceid={deviceId}&lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
            return result.Params.Switches[channel ?? 0].IsPoweredOn;
        }

        public Task<bool> ToggleDeviceAsync(string deviceId, int? channel)
        {
            throw new NotImplementedException();
        }
    }
}
