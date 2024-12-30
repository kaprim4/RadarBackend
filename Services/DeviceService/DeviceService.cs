using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DeviceService
{
    public class DeviceService : AService, IDeviceService
    {

        public DeviceService(AppDbContext context) : base(context)
        {
        }

        public async Task<ResponseModel<DeviceDTO>> List(PagableDTO<DeviceDTO> pagable)
        {
            var output = new ResponseModel<DeviceDTO>();

            var query = _context.Devices.Select(x => new DeviceDTO()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                SerialNumber = x.SerialNumber
            });
            output.Pagable.TotalContent = await query.CountAsync();
            output.Pagable.Content = await query.Filter(pagable).ToListAsync();


            return output;
        }

        public async Task<DeviceDTO?> GetById(string serialNumber)
        {
            return await _context.Devices.Where(x=>x.SerialNumber == serialNumber).Select(x => new DeviceDTO()
            {
                Name = x.Name,
                IsActive = x.IsActive,
                SerialNumber = x.SerialNumber
            }).FirstOrDefaultAsync();
        }

        public async Task<ResponseModel<DeviceDTO>> Add(DeviceDTO device)
        {
            ResponseModel<DeviceDTO> output = new(true);
            var divice = await _context.Devices.FirstOrDefaultAsync(x => x.SerialNumber == device.SerialNumber);
            if (divice != null)
            {
                output.AddErrorMessage("device déjà ajouter");
            }
            else
            {
                var entry = new Device()
                {
                    IsActive = device.IsActive,
                    Name = GenerateRadarRefrence(),
                    SerialNumber = device.SerialNumber
                };
                try
                {
                    _context.Devices.Add(entry);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    output.AddErrorMessage(ex.ToString());
                }
            }
            
            
            return output;
        }

        public async Task<ResponseModel<DeviceDTO>> Update(DeviceDTO updatedDevice)
        {
            ResponseModel<DeviceDTO> output = new(true);
            var existingDevice = await _context.Devices.FirstOrDefaultAsync(x=>x.Id == updatedDevice.Id);
            if (existingDevice == null)
            {
                output.AddErrorMessage("Composant introuvable");
            }
            else
            {
                //existingDevice.Name = updatedDevice.Name;
                existingDevice.SerialNumber = updatedDevice.SerialNumber;
                existingDevice.IsActive = updatedDevice.IsActive;
                try
                {
                    _context.Devices.Update(existingDevice);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    output.AddErrorMessage(ex.ToString());
                }
                
            }

            return output;
        }

        public async Task<ResponseModel<DeviceDTO>> Delete(string serialNumber)
        {
            ResponseModel<DeviceDTO> output = new(true);
            using var transaction = await _context.Database.BeginTransactionAsync();
            var device = await _context.Devices.Include(x=>x.Lots).FirstOrDefaultAsync(x=>x.SerialNumber == serialNumber);
            if (device == null)
            {
                output.AddErrorMessage("Composant introuvable");
            }
            else
            {
                try
                {
                    foreach (var item in device.Lots)
                    {
                        _context.Lots.Remove(item);
                    }

                    _context.Devices.Remove(device);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    output.AddErrorMessage(ex.ToString());
                    await transaction.RollbackAsync();
                }

            }


            return output;
        }

        public string GenerateRadarRefrence()
        {
            var lastLotId = _context.Devices.Max(x => (int?)x.Id) ?? 0;
            lastLotId++;
            return $"R-{lastLotId.ToString().PadLeft(6, '0')}-{DateTime.Now:ddMMyy}";
        }
    }
}
