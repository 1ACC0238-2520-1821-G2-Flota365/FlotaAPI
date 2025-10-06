using BackendWeb.Fleets.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Fleets.Infrastructure.Repositories
{
    public class FleetRepository : IFleetRepository
    {
        private readonly AppDbContext _context;

        public FleetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Fleet?> GetByIdAsync(int id)
        {
            return await _context.Fleets.FindAsync(id);
        }

        public async Task<List<Fleet>> GetAllAsync()
        {
            return await _context.Fleets
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Fleet> AddAsync(Fleet fleet)
        {
            _context.Fleets.Add(fleet);
            await _context.SaveChangesAsync();
            return fleet;
        }

        public async Task<Fleet> UpdateAsync(Fleet fleet)
        {
            _context.Fleets.Update(fleet);
            await _context.SaveChangesAsync();
            return fleet;
        }

        public async Task DeleteAsync(int id)
        {
            var fleet = await GetByIdAsync(id);
            if (fleet != null)
            {
                _context.Fleets.Remove(fleet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Fleets.AnyAsync(f => f.Id == id);
        }
    }
}
