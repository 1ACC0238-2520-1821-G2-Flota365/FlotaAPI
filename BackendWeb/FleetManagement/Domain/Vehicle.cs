namespace BackendWeb.FleetManagement.Domain
{ 
    public class Vehicle
    {
        public int Id { get; private set; }
        public string LicensePlate { get; private set; } = string.Empty;
        public string Brand { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public int Mileage { get; private set; }
        public int Status { get; private set; } = 1; // 1 = Active, 0 = Inactive
        public string StatusName { get; private set; } = "Active";
        public int FleetId { get; private set; }
        public string FleetName { get; private set; } = string.Empty;
        public int DriverId { get; private set; }
        public string DriverName { get; private set; } = string.Empty;
        public DateTime? LastServiceDate { get; private set; }
        public DateTime? NextServiceDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Constructor protegido para EF
        protected Vehicle() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Vehicle(string licensePlate, string brand, string model, int year, int mileage, 
                      int fleetId, string fleetName)
        {
            LicensePlate = licensePlate;
            Brand = brand;
            Model = model;
            Year = year;
            Mileage = mileage;
            Status = 1;
            StatusName = "Active";
            FleetId = fleetId;
            FleetName = fleetName;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(int newStatus, string statusName)
        {
            Status = newStatus;
            StatusName = statusName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignDriver(int driverId, string driverName)
        {
            DriverId = driverId;
            DriverName = driverName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateMileage(int newMileage)
        {
            Mileage = newMileage;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetServiceDates(DateTime? lastServiceDate, DateTime? nextServiceDate)
        {
            LastServiceDate = lastServiceDate;
            NextServiceDate = nextServiceDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
