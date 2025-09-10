using Management.Domain;

namespace BackendWeb.Management.Application
{
    public class ManagerService
    {
        private readonly IManagerRepository _repository;

        public ManagerService(IManagerRepository repository) => _repository = repository;

        public async Task<Manager> Register(string name, string email)
        {
            var manager = new Manager(name, email);
            await _repository.AddAsync(manager);
            return manager;
        }

        public async Task<List<Manager>> GetAll() => await _repository.GetAllAsync();
    }
}
