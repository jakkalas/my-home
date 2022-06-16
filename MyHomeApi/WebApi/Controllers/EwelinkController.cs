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
        [Route("/devices")]
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _eweLinkService.GetAllDevicesAsync();
        }
    }
}
