namespace BackendWeb.FleetManagement.Domain
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllAsync();
    }
}
