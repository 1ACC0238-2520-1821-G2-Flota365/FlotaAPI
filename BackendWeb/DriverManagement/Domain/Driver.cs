namespace BackendWeb.DriverManagement.Domain
{
    public class Driver
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string LicenseNumber { get; private set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; private set; }
        public string Phone { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public int ExperienceYears { get; private set; }
        public int Status { get; private set; } = 1; // 1 = Active, 0 = Inactive
        public string StatusName { get; private set; } = "Active";
        public string AssignedVehicle { get; private set; } = string.Empty;
        public bool IsActive => Status == 1;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsLicenseExpiringSoon => LicenseExpiryDate <= DateTime.UtcNow.AddDays(30);

        protected Driver() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Driver(string code, string firstName, string lastName, string licenseNumber, 
                     DateTime licenseExpiryDate, string phone, string email, int experienceYears)
        {
            Code = code;
            FirstName = firstName;
            LastName = lastName;
            LicenseNumber = licenseNumber;
            LicenseExpiryDate = licenseExpiryDate;
            Phone = phone;
            Email = email;
            ExperienceYears = experienceYears;
            Status = 1;
            StatusName = "Active";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(int status, string statusName)
        {
            Status = status;
            StatusName = statusName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignVehicle(string vehicleInfo)
        {
            AssignedVehicle = vehicleInfo;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePersonalInfo(string firstName, string lastName, string phone, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLicense(string licenseNumber, DateTime licenseExpiryDate)
        {
            LicenseNumber = licenseNumber;
            LicenseExpiryDate = licenseExpiryDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateExperience(int experienceYears)
        {
            ExperienceYears = experienceYears;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(int id);
        Task<List<Driver>> GetAllAsync();
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
    }
}
