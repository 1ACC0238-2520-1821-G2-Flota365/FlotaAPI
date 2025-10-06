namespace Management.Domain
{
    public class Manager
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Status { get; private set; } = "ACTIVE";

        protected Manager() { }

        public Manager(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
    }

    public interface IManagerRepository
    {
        Task<Manager?> GetByIdAsync(Guid id);
        Task<List<Manager>> GetAllAsync();
        Task AddAsync(Manager manager);
    }
}
