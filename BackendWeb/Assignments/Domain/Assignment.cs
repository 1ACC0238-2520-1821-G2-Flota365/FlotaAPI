namespace BackendWeb.Assignments.Domain
{
    public class Assignment
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid DriverId { get; private set; }
        public string Route { get; private set; } = string.Empty;
        public string Status { get; private set; } = "PENDING"; // PENDING, IN_PROGRESS, COMPLETED
        public DateTime AssignedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Constructor protegido para EF
        protected Assignment() { }

        public Assignment(Guid vehicleId, Guid driverId, string route)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            DriverId = driverId;
            Route = route;
            Status = "PENDING";
            AssignedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != "PENDING")
                throw new InvalidOperationException("Solo las asignaciones pendientes pueden iniciarse.");

            Status = "IN_PROGRESS";
        }

        public void Complete()
        {
            if (Status != "IN_PROGRESS")
                throw new InvalidOperationException("Solo las asignaciones en progreso pueden completarse.");

            Status = "COMPLETED";
            CompletedAt = DateTime.UtcNow;
        }
    }

    // Repositorio de dominio
    public interface IAssignmentRepository
    {
        Task<Assignment?> GetByIdAsync(Guid id);
        Task<List<Assignment>> GetAllAsync();
        Task AddAsync(Assignment assignment);
        Task UpdateAsync(Assignment assignment);
    }
}
