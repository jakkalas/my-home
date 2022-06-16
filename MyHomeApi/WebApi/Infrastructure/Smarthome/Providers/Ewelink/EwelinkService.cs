using MyHomeApi.Infrastructure.Smarthome.Models;

namespace MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink
{
    public class EwelinkService : ISmartHomeService
    {
        public Device[] GetAllDevices()
        {
            throw new NotImplementedException();
        }

        public int GetDeviceChannelCount()
        {
            throw new NotImplementedException();
        }

        public bool GetDevicePowerState(int? channel)
        {
            throw new NotImplementedException();
        }

        public bool ToggleDevice(int? channel)
        {
            throw new NotImplementedException();
        }
    }
}
