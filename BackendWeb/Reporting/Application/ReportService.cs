using Reporting.Domain;

namespace BackendWeb.Reporting.Application
{
    public class ReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository) => _repository = repository;

        public async Task<Report> Generate(string title, string type)
        {
            var report = new Report(title, type);
            await _repository.AddAsync(report);
            return report;
        }

        public async Task<List<Report>> GetAll() => await _repository.GetAllAsync();
    }
}
