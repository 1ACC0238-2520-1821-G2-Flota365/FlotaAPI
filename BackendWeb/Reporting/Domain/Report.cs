namespace Reporting.Domain
{
    public class Report
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public DateTime GeneratedAt { get; private set; }

        protected Report() { }

        public Report(string title, string type)
        {
            Title = title;
            Type = type;
            GeneratedAt = DateTime.UtcNow;
        }
    }

    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(int id);
        Task<List<Report>> GetAllAsync();
        Task AddAsync(Report report);
    }
}
