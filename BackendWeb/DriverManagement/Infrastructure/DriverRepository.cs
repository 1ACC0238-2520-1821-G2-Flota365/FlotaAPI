using BackendWeb.DriverManagement.Domain;
using Shared.Persistence.EFC;
using Microsoft.EntityFrameworkCore;

namespace DriverManagement.Infrastructure.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context) => _context = context;

        public async Task<Driver?> GetByIdAsync(int id) => await _context.Drivers.FindAsync(id);

        public async Task<List<Driver>> GetAllAsync() => await _context.Drivers.ToListAsync();

        public async Task AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }
    }
}
