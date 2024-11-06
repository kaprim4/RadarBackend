using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RadarBackend.Models;
using RadarBackend.Services;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult<Device>> CreateDevice(Device device)
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(device);
            return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id }, createdDevice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, Device device)
        {
            var result = await _deviceService.UpdateDeviceAsync(id, device);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var result = await _deviceService.DeleteDeviceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
