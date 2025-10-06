namespace BackendWeb.DriverManagement.Application.DTOs
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string AssignedVehicle { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsLicenseExpiringSoon { get; set; }
    }

    public class CreateDriverDto
    {
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
    }

    public class UpdateDriverDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public int ExperienceYears { get; set; }
        public string AssignedVehicle { get; set; } = string.Empty;
    }

    public class DriverStatsDto
    {
        public int TotalDrivers { get; set; }
        public int ActiveDrivers { get; set; }
        public int InactiveDrivers { get; set; }
        public int DriversWithExpiredLicense { get; set; }
        public int AssignedDrivers { get; set; }
        public int UnassignedDrivers { get; set; }
        public double AverageExperience { get; set; }
    }
}
