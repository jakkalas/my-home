using MyHomeApi.Infrastructure.Smarthome.Models;

namespace MyHomeApi.Infrastructure.Smarthome.Providers
{
    public interface ISmartHomeService
    {
        Device[] GetAllDevices();
        int GetDeviceChannelCount();
        bool GetDevicePowerState(int? channel);
        bool ToggleDevice(int? channel);
    }
}
