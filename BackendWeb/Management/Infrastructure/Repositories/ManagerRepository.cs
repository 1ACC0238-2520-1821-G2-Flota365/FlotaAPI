using Management.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Management.Infrastructure.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly AppDbContext _context;

        public ManagerRepository(AppDbContext context) => _context = context;

        public async Task<Manager?> GetByIdAsync(Guid id) =>
            await _context.Managers.FindAsync(id);

        public async Task<List<Manager>> GetAllAsync() =>
            await _context.Managers.ToListAsync();

        public async Task AddAsync(Manager manager)
        {
            await _context.Managers.AddAsync(manager);
            await _context.SaveChangesAsync();
        }
    }
}
