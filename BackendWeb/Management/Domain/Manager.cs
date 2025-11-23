namespace Management.Domain
{
    public class Manager
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Status { get; private set; } = "ACTIVE";

        protected Manager() { }

        public Manager(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }

    public interface IManagerRepository
    {
        Task<Manager?> GetByIdAsync(int id);
        Task<List<Manager>> GetAllAsync();
        Task AddAsync(Manager manager);
    }
}
