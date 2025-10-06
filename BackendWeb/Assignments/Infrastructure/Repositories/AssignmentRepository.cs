using BackendWeb.Assignments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Assignments.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context) => _context = context;

        public async Task<Assignment?> GetByIdAsync(Guid id) =>
            await _context.Assignments.FindAsync(id);

        public async Task<List<Assignment>> GetAllAsync() =>
            await _context.Assignments.ToListAsync();

        public async Task AddAsync(Assignment assignment)
        {
            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
