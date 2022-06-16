using MyHomeApi.Infrastructure.Smarthome.Models;
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
            return await _httpClient.GetFromJsonAsync<IEnumerable<Device>>($"user/device?lang=en&appid={_configuration["EweLink:AppId"]}&version=8&getTags=1");
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetDeviceChannelCountAsync(string deviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetDevicePowerStateAsync(string deviceId, int? channel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ToggleDeviceAsync(string deviceId, int? channel)
        {
            throw new NotImplementedException();
        }
    }
}
