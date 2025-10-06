using BackendWeb.DriverManagement.Domain;
using BackendWeb.DriverManagement.Application.DTOs;

namespace DriverManagement.Application.Services
{
    public class DriverService
    {
        private readonly IDriverRepository _repository;

        public DriverService(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<DriverDto> RegisterDriverAsync(CreateDriverDto createDto)
        {
            var driver = new Driver(createDto.Code, createDto.FirstName, createDto.LastName, 
                                   createDto.LicenseNumber, createDto.LicenseExpiryDate,
                                   createDto.Phone, createDto.Email, createDto.ExperienceYears);
            await _repository.AddAsync(driver);
            return MapToDto(driver);
        }

        public async Task<List<DriverDto>> GetAllDriversAsync()
        {
            var drivers = await _repository.GetAllAsync();
            return drivers.Select(MapToDto).ToList();
        }

        public async Task<DriverDto?> GetDriverByIdAsync(int id)
        {
            var driver = await _repository.GetByIdAsync(id);
            return driver != null ? MapToDto(driver) : null;
        }

        public async Task<DriverDto?> UpdateDriverAsync(int id, UpdateDriverDto updateDto)
        {
            var driver = await _repository.GetByIdAsync(id);
            if (driver == null) return null;

            // Update driver properties
            driver.UpdatePersonalInfo(updateDto.FirstName, updateDto.LastName, updateDto.Phone, updateDto.Email);
            driver.UpdateLicense(updateDto.LicenseNumber, updateDto.LicenseExpiryDate);
            
            if (!string.IsNullOrEmpty(updateDto.AssignedVehicle))
            {
                driver.AssignVehicle(updateDto.AssignedVehicle);
            }

            await _repository.UpdateAsync(driver);
            return MapToDto(driver);
        }

        public async Task<bool> DeleteDriverAsync(int id)
        {
            var driver = await _repository.GetByIdAsync(id);
            if (driver == null) return false;

            // Soft delete - mark as inactive
            driver.UpdateStatus(0, "Inactive");
            await _repository.UpdateAsync(driver);

            return true;
        }

        public async Task<DriverStatsDto> GetDriverStatsAsync()
        {
            var drivers = await _repository.GetAllAsync();
            
            var totalDrivers = drivers.Count;
            var activeDrivers = drivers.Count(d => d.IsActive);
            var inactiveDrivers = drivers.Count(d => !d.IsActive);
            var driversWithExpiredLicense = drivers.Count(d => d.IsLicenseExpiringSoon);
            var assignedDrivers = drivers.Count(d => !string.IsNullOrEmpty(d.AssignedVehicle));
            var unassignedDrivers = drivers.Count(d => string.IsNullOrEmpty(d.AssignedVehicle));

            return new DriverStatsDto
            {
                TotalDrivers = totalDrivers,
                ActiveDrivers = activeDrivers,
                InactiveDrivers = inactiveDrivers,
                DriversWithExpiredLicense = driversWithExpiredLicense,
                AssignedDrivers = assignedDrivers,
                UnassignedDrivers = unassignedDrivers,
                AverageExperience = drivers.Any() ? drivers.Average(d => d.ExperienceYears) : 0
            };
        }

        private static DriverDto MapToDto(Driver driver)
        {
            return new DriverDto
            {
                Id = driver.Id,
                Code = driver.Code,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                FullName = driver.FullName,
                LicenseNumber = driver.LicenseNumber,
                LicenseExpiryDate = driver.LicenseExpiryDate,
                Phone = driver.Phone,
                Email = driver.Email,
                ExperienceYears = driver.ExperienceYears,
                Status = driver.Status,
                StatusName = driver.StatusName,
                AssignedVehicle = driver.AssignedVehicle,
                IsActive = driver.IsActive,
                CreatedAt = driver.CreatedAt,
                UpdatedAt = driver.UpdatedAt,
                IsLicenseExpiringSoon = driver.IsLicenseExpiringSoon
            };
        }
    }
}
