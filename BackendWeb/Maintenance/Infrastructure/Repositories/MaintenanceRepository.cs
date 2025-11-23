using BackendWeb.Maintenance.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace Maintenance.Infrastructure.Repositories
{
    public class MaintenanceRepository : IMaintenanceRecordRepository, IMaintenanceRepository
    {
        private readonly AppDbContext _context;

        public MaintenanceRepository(AppDbContext context)
        {
            _context = context;
        }

        // MaintenanceRecord methods
        public async Task<MaintenanceRecord?> GetByIdAsync(int id)
        {
            return await _context.MaintenanceRecords.FindAsync(id);
        }

        public async Task<List<MaintenanceRecord>> GetAllAsync()
        {
            return await _context.MaintenanceRecords.ToListAsync();
        }

        public async Task<List<MaintenanceRecord>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRecord>> GetOverdueAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.MaintenanceRecords
                .Where(m => m.Status != MaintenanceStatus.Completed && m.ScheduledDate < now)
                .OrderBy(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<MaintenanceRecord> AddAsync(MaintenanceRecord record)
        {
            _context.MaintenanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<MaintenanceRecord> UpdateAsync(MaintenanceRecord record)
        {
            _context.MaintenanceRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record != null)
            {
                _context.MaintenanceRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
