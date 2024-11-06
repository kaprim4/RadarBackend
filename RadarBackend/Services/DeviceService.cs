using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RadarBackend.Data;
using RadarBackend.Models;

namespace RadarBackend.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task<Device> CreateDeviceAsync(Device device);
        Task<bool> UpdateDeviceAsync(int id, Device updatedDevice);
        Task<bool> DeleteDeviceAsync(int id);
    }

    public class DeviceService : IDeviceService
    {
        private readonly ApplicationDbContext _context;

        public DeviceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        public async Task<Device> CreateDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> UpdateDeviceAsync(int id, Device updatedDevice)
        {
            var existingDevice = await _context.Devices.FindAsync(id);
            if (existingDevice == null)
            {
                return false;
            }

            existingDevice.Name = updatedDevice.Name;
            existingDevice.SerialNumber = updatedDevice.SerialNumber;
            existingDevice.IsActive = updatedDevice.IsActive;

            _context.Devices.Update(existingDevice);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
