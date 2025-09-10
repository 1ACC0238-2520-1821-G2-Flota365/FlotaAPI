namespace BackendWeb.Fleets.Domain
{
    public enum FleetType
    {
        Primary = 1,
        Secondary = 2,
        External = 3,
        Specialized = 4
    }

    public class Fleet
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public FleetType Type { get; private set; }
        public string TypeName { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;
        public int VehicleCount { get; private set; }
        public int ActiveVehicles { get; private set; }
        public int InMaintenanceVehicles { get; private set; }
        public decimal Performance { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        
        // Calculated properties
        public decimal PerformancePercentage => Performance * 100;
        public string StatusText => IsActive ? "Active" : "Inactive";
        public decimal VehicleUtilization => VehicleCount > 0 ? Math.Round((decimal)ActiveVehicles / VehicleCount * 100, 2) : 0;

        // Constructor protegido para EF
        protected Fleet() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Fleet(string name, string description, FleetType type)
        {
            Name = name;
            Description = description;
            Type = type;
            TypeName = type.ToString();
            Code = GenerateCode(name, type);
            IsActive = true;
            VehicleCount = 0;
            ActiveVehicles = 0;
            InMaintenanceVehicles = 0;
            Performance = 0.0m;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDetails(string name, string description, FleetType type)
        {
            Name = name;
            Description = description;
            Type = type;
            TypeName = type.ToString();
            Code = GenerateCode(name, type);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateVehicleStats(int vehicleCount, int activeVehicles, int inMaintenanceVehicles)
        {
            VehicleCount = vehicleCount;
            ActiveVehicles = activeVehicles;
            InMaintenanceVehicles = inMaintenanceVehicles;
            
            // Calculate performance based on vehicle utilization
            Performance = VehicleCount > 0 ? (decimal)ActiveVehicles / VehicleCount : 0.0m;
            
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePerformance(decimal performance)
        {
            Performance = Math.Max(0, Math.Min(1, performance)); // Ensure it's between 0 and 1
            UpdatedAt = DateTime.UtcNow;
        }

        private string GenerateCode(string name, FleetType type)
        {
            var typePrefix = type switch
            {
                FleetType.Primary => "PRI",
                FleetType.Secondary => "SEC",
                FleetType.External => "EXT",
                FleetType.Specialized => "SPE",
                _ => "GEN"
            };

            var namePrefix = name.Length >= 3 
                ? name.Substring(0, 3).ToUpper() 
                : name.ToUpper().PadRight(3, 'X');

            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
            return $"{typePrefix}-{namePrefix}-{timestamp}";
        }
    }

    // Repository interface
    public interface IFleetRepository
    {
        Task<Fleet?> GetByIdAsync(int id);
        Task<List<Fleet>> GetAllAsync();
        Task<Fleet> AddAsync(Fleet fleet);
        Task<Fleet> UpdateAsync(Fleet fleet);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
