using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DeviceService
{
    public interface IDeviceService
    {
        Task<ResponseModel<DeviceDTO>> List(PagableDTO<DeviceDTO> pagable);
        Task<DeviceDTO> GetById(string serialNumber);
        Task<ResponseModel<DeviceDTO>> Add(DeviceDTO device);
        Task<ResponseModel<DeviceDTO>> Update(DeviceDTO updatedDevice);
        Task<ResponseModel<DeviceDTO>> Delete(string serialNumber);
    }
}
