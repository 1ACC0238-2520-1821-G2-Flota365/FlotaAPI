namespace BackendWeb.FleetManagement.Application.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int FleetId { get; set; }
        public string FleetName { get; set; } = string.Empty;
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public DateTime? LastServiceDate { get; set; }
        public DateTime? NextServiceDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateVehicleDto
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public int FleetId { get; set; }
        public string FleetName { get; set; } = string.Empty;
    }

    public class UpdateVehicleDto
    {
        public int Mileage { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public DateTime? LastServiceDate { get; set; }
        public DateTime? NextServiceDate { get; set; }
    }
}
