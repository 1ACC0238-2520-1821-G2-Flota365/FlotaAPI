using System.ComponentModel.DataAnnotations;

namespace BackendWeb.Maintenance.Domain
{
    public enum MaintenanceType
    {
        Preventive = 1,
        Corrective = 2,
        Emergency = 3,
        Inspection = 4
    }

    public enum MaintenanceStatus
    {
        Scheduled = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }

    public class MaintenanceRecord
    {
        public int Id { get; private set; }
        public int VehicleId { get; private set; }
        public string VehicleLicensePlate { get; private set; } = string.Empty;
        public string VehicleModel { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public MaintenanceType Type { get; private set; }
        public string TypeName { get; private set; } = string.Empty;
        public decimal Cost { get; private set; }
        public DateTime ScheduledDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public MaintenanceStatus Status { get; private set; }
        public string StatusName { get; private set; } = string.Empty;
        public string Notes { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsOverdue => Status != MaintenanceStatus.Completed && DateTime.UtcNow > ScheduledDate;
        public int DaysOverdue => IsOverdue ? (int)(DateTime.UtcNow - ScheduledDate).TotalDays : 0;

        // Constructor protegido para EF
        protected MaintenanceRecord() { }

        public MaintenanceRecord(int vehicleId, string description, MaintenanceType type, decimal cost, DateTime scheduledDate, string notes = "")
        {
            VehicleId = vehicleId;
            Description = description;
            Type = type;
            TypeName = type.ToString();
            Cost = cost;
            ScheduledDate = scheduledDate;
            Status = MaintenanceStatus.Scheduled;
            StatusName = Status.ToString();
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDetails(string description, MaintenanceType type, decimal cost, DateTime scheduledDate, string notes)
        {
            Description = description;
            Type = type;
            TypeName = type.ToString();
            Cost = cost;
            ScheduledDate = scheduledDate;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(MaintenanceStatus status, DateTime? completedDate = null)
        {
            Status = status;
            StatusName = status.ToString();
            CompletedDate = completedDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetVehicleInfo(string licensePlate, string model)
        {
            VehicleLicensePlate = licensePlate;
            VehicleModel = model;
        }
    }

    public class ServiceRecord
    {
        public int Id { get; private set; }
        public int VehicleId { get; private set; }
        public string VehicleLicensePlate { get; private set; } = string.Empty;
        public string ServiceType { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Cost { get; private set; }
        public DateTime ServiceDate { get; private set; }
        public int MileageAtService { get; private set; }
        public string ServiceProvider { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        // Constructor protegido para EF
        protected ServiceRecord() { }

        public ServiceRecord(int vehicleId, string serviceType, string description, decimal cost, DateTime serviceDate, int mileageAtService, string serviceProvider)
        {
            VehicleId = vehicleId;
            ServiceType = serviceType;
            Description = description;
            Cost = cost;
            ServiceDate = serviceDate;
            MileageAtService = mileageAtService;
            ServiceProvider = serviceProvider;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetVehicleInfo(string licensePlate)
        {
            VehicleLicensePlate = licensePlate;
        }
    }

    // Repository interfaces
    public interface IMaintenanceRecordRepository
    {
        Task<MaintenanceRecord?> GetByIdAsync(int id);
        Task<List<MaintenanceRecord>> GetAllAsync();
        Task<List<MaintenanceRecord>> GetByVehicleIdAsync(int vehicleId);
        Task<List<MaintenanceRecord>> GetOverdueAsync();
        Task<MaintenanceRecord> AddAsync(MaintenanceRecord record);
        Task<MaintenanceRecord> UpdateAsync(MaintenanceRecord record);
        Task DeleteAsync(int id);
    }

    public interface IServiceRecordRepository
    {
        Task<ServiceRecord?> GetByIdAsync(int id);
        Task<List<ServiceRecord>> GetAllAsync();
        Task<List<ServiceRecord>> GetByVehicleIdAsync(int vehicleId);
        Task<ServiceRecord> AddAsync(ServiceRecord record);
        Task DeleteAsync(int id);
    }

    // Keep the old interface for compatibility with existing services
    public interface IMaintenanceRepository : IMaintenanceRecordRepository
    {
    }

    // Legacy MaintenanceOrder class for backward compatibility
    public class MaintenanceOrder
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "OPEN";
        public string Notes { get; set; } = string.Empty;
        public double TotalCost { get; set; }
    }
}
