using MyHomeApi.Infrastructure.Smarthome.Models;

namespace MyHomeApi.Infrastructure.Smarthome.Providers
{
    public interface ISmartHomeService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device> GetDeviceAsync(string deviceId);
        Task<int> GetDeviceChannelCountAsync(string deviceId);
        Task<bool> GetDevicePowerStateAsync(string deviceId, int? channel);
        Task<bool> ToggleDeviceAsync(string deviceId, int? channel);
    }
}
