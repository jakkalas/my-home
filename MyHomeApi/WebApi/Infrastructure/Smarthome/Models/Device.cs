namespace MyHomeApi.Infrastructure.Smarthome.Models
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public DeviceType Type { get; set; }
    }
}
