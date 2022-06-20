using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink;

namespace MyHomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EwelinkController : ControllerBase
    {
        private readonly EweLinkService _eweLinkService;

        public EwelinkController(EweLinkService eweLinkService)
        {
            _eweLinkService = eweLinkService;
        }

        [HttpGet]
        [Route("devices")]
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _eweLinkService.GetAllDevicesAsync();
        }

        [HttpGet]
        [Route("device/{deviceId}")]
        public async Task<Device> GetDeviceByDeviceIdAsync(string deviceId)
        {
            return await _eweLinkService.GetDeviceAsync(deviceId);
        }

        [HttpGet]
        [Route("device/ispoweredon")]
        public async Task<bool> GetIsDevicePowerOnAsync(string deviceId, int? channel)
        {
            return await _eweLinkService.GetIsDevicePowerOn(deviceId, channel);
        }
    }
}
