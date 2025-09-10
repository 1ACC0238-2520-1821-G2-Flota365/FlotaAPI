namespace BackendWeb.Maintenance.Application.DTOs
{
    public class MaintenanceRecordDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehicleLicensePlate { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
    }

    public class CreateMaintenanceRecordRequest
    {
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public decimal Cost { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateMaintenanceRecordRequest
    {
        public string Description { get; set; } = string.Empty;
        public int Type { get; set; }
        public decimal Cost { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
