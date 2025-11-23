using Microsoft.EntityFrameworkCore;
using Reporting.Domain;
using Shared.Persistence.EFC;

namespace BackendWeb.Reporting.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context) => _context = context;

        public async Task<Report?> GetByIdAsync(int id) =>
            await _context.Reports.FindAsync(id);

        public async Task<List<Report>> GetAllAsync() =>
            await _context.Reports.ToListAsync();

        public async Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }
    }
}
