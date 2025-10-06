using BackendWeb.Maintenance.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Maintenance.Infrastructure.Repositories
{
    public class ServiceRecordRepository : IServiceRecordRepository
    {
        private readonly AppDbContext _context;

        public ServiceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRecord?> GetByIdAsync(int id)
        {
            return await _context.ServiceRecords.FindAsync(id);
        }

        public async Task<List<ServiceRecord>> GetAllAsync()
        {
            return await _context.ServiceRecords.ToListAsync();
        }

        public async Task<List<ServiceRecord>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _context.ServiceRecords
                .Where(s => s.VehicleId == vehicleId)
                .OrderByDescending(s => s.ServiceDate)
                .ToListAsync();
        }

        public async Task<ServiceRecord> AddAsync(ServiceRecord record)
        {
            _context.ServiceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record != null)
            {
                _context.ServiceRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
