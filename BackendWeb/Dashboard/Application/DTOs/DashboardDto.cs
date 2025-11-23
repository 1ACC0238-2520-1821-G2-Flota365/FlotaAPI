namespace BackendWeb.Dashboard.Application.DTOs
{
    /// <summary>
    /// Dashboard statistics DTO
    /// </summary>
    public class DashboardStatsDto
    {
        public int TotalVehicles { get; set; }
        public int ActiveDrivers { get; set; }
        public int VehiclesInMaintenance { get; set; }
        public decimal FleetEfficiency { get; set; }
        public string TotalVehiclesChange { get; set; } = string.Empty;
        public string ActiveDriversChange { get; set; } = string.Empty;
        public string MaintenanceChange { get; set; } = string.Empty;
        public string EfficiencyChange { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public int TotalFleets { get; set; }
        public int AlertsCount { get; set; }
        public decimal AverageVehicleAge { get; set; }
        public int VehiclesDueForService { get; set; }
    }

    /// <summary>
    /// Active vehicle DTO for dashboard
    /// </summary>
    public class ActiveVehicleDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string FleetName { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string StatusColor { get; set; } = string.Empty;
    }

    /// <summary>
    /// Fleet summary DTO for dashboard
    /// </summary>
    public class FleetSummaryDto
    {
        public int TotalFleets { get; set; }
        public int PrimaryFleetVehicles { get; set; }
        public int SecondaryFleetVehicles { get; set; }
        public int ExternalFleetVehicles { get; set; }
        public decimal PrimaryFleetEfficiency { get; set; }
        public decimal SecondaryFleetEfficiency { get; set; }
        public decimal ExternalFleetEfficiency { get; set; }
        public decimal OverallEfficiency { get; set; }
        public string PrimaryFleetTrend { get; set; } = string.Empty;
        public string SecondaryFleetTrend { get; set; } = string.Empty;
        public string ExternalFleetTrend { get; set; } = string.Empty;
    }
}
