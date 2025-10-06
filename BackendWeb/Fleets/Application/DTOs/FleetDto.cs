namespace BackendWeb.Fleets.Application.DTOs
{
    /// <summary>
    /// Fleet response DTO
    /// </summary>
    public class FleetDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int VehicleCount { get; set; }
        public int ActiveVehicles { get; set; }
        public int InMaintenanceVehicles { get; set; }
        public decimal Performance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal PerformancePercentage { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public decimal VehicleUtilization { get; set; }
    }

    /// <summary>
    /// Create fleet request DTO
    /// </summary>
    public class CreateFleetRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
    }

    /// <summary>
    /// Update fleet request DTO
    /// </summary>
    public class UpdateFleetRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}
