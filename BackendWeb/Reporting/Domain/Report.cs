namespace Reporting.Domain
{
    public class Report
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public DateTime GeneratedAt { get; private set; }

        protected Report() { }

        public Report(string title, string type)
        {
            Id = Guid.NewGuid();
            Title = title;
            Type = type;
            GeneratedAt = DateTime.UtcNow;
        }
    }

    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(Guid id);
        Task<List<Report>> GetAllAsync();
        Task AddAsync(Report report);
    }
}
