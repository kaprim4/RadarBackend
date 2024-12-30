using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DeviceService;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/devices")]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("List")]
        public async Task<ActionResult<ResponseModel<DeviceDTO>>> List(PagableDTO<DeviceDTO> pagable)
        {
            return Ok(await _deviceService.List(pagable));
        }

        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<Device>> GetById(string serialNumber)
        {
            DeviceDTO device = await _deviceService.GetById(serialNumber);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<Device>> Add(DeviceDTO device)
        {
            var response = await _deviceService.Add(device);
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDevice(DeviceDTO device)
        {
            var response = await _deviceService.Update(device);
            
            return Ok(response);
        }

        [HttpDelete("Delete/{serialNumber}")]
        public async Task<IActionResult> DeleteDevice(string serialNumber)
        {
            var result = await _deviceService.Delete(serialNumber);
            
            return Ok(result);
        }
    }
}
