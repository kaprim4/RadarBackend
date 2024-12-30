using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using Domain.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.DataService;
using Services.DeviceService;
using static System.Net.Mime.MediaTypeNames;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/data")]
    [Authorize]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("List")]
        public async Task<ActionResult<ResponseModel<LotListDTO>>> All(PagableDTO<LotListDTO> pagable)
        {
            return Ok(await _dataService.List(pagable));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TreatmentDTO>> GetById(int id)
        {
            var lot = await _dataService.GetById(id);
            if (lot == null)
            {
                return NotFound();
            }
            return Ok(lot);
        }

        [HttpPost("checkfiles")]
        public async Task<ActionResult<TreatmentDTO>> CheckFiles(List<string> files)
        {
            return Ok(await _dataService.CheckFiles(files));
        }

        

        [HttpPost("Add")]
        public async Task<ActionResult<TreatmentDTO>> Add(LotDTO obj)
        {
            //foreach (var item in obj.Deployments)
            //{
            //    if(item.VehicleDatas != null && item.VehicleDatas.Any())
            //        foreach (var vehicle in item.VehicleDatas)
            //        {
            //            if (vehicle.ImageBytes != null)
            //            {
            //                var imagePath = Path.Combine(ImageHelper.GetResourcePathStatic(), vehicle.ImageBytes.FileName);
            //                using var stream = new FileStream(imagePath, FileMode.Create);
            //                await vehicle.ImageBytes.CopyToAsync(stream);
            //            }

            //            if (vehicle.VideoBytes != null)
            //            {
            //                var videoPath = Path.Combine(ImageHelper.GetResourcePathStatic(true), vehicle.VideoBytes.FileName);
            //                using var stream = new FileStream(videoPath, FileMode.Create);
            //                await vehicle.VideoBytes.CopyToAsync(stream);
            //            }
            //        }
            //}

            var response = await _dataService.Add(obj);
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(LotDTO device)
        {
            var response = await _dataService.Update(device);

            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dataService.Delete(id);

            return Ok(result);
        }

        [HttpDelete("deletedocument/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _dataService.DeleteDocument(id);

            return Ok(result);
        }

        [HttpPost("adddocument")]
        public async Task<ActionResult<TreatmentDTO>> AddDocument([FromForm] List<IFormFile> Images, [FromForm] string Dto)
        {
            var obj = JsonConvert.DeserializeObject<DocumentDTO>(Dto);
            return Ok(await _dataService.AddDocument(obj));

        }
    }
}
