using BackendWeb.FleetManagement.Application.DTOs;
using BackendWeb.FleetManagement.Domain;

namespace FleetManagement.Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _repository;

        public VehicleService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<VehicleDto> RegisterVehicleAsync(CreateVehicleDto createDto)
        {
            var vehicle = new Vehicle(createDto.LicensePlate, createDto.Brand, createDto.Model, 
                                    createDto.Year, createDto.Mileage, createDto.FleetId, createDto.FleetName);
            await _repository.AddAsync(vehicle);

            return MapToDto(vehicle);
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _repository.GetAllAsync();
            return vehicles.Select(MapToDto).ToList();
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            return vehicle != null ? MapToDto(vehicle) : null;
        }

        public async Task<VehicleDto?> UpdateVehicleAsync(int id, UpdateVehicleDto updateDto)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            if (vehicle == null) return null;

            // Update vehicle properties
            vehicle.UpdateMileage(updateDto.Mileage);
            vehicle.SetServiceDates(updateDto.LastServiceDate, updateDto.NextServiceDate);
            
            if (updateDto.DriverId != 0)
            {
                vehicle.AssignDriver(updateDto.DriverId, updateDto.DriverName);
            }

            if (updateDto.Status != 0)
            {
                vehicle.UpdateStatus(updateDto.Status, updateDto.StatusName);
            }

            await _repository.UpdateAsync(vehicle);
            return MapToDto(vehicle);
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            if (vehicle == null) return false;

            // Soft delete - mark as inactive
            vehicle.UpdateStatus(0, "Inactive");
            await _repository.UpdateAsync(vehicle);

            return true;
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                Status = vehicle.Status,
                StatusName = vehicle.StatusName,
                FleetId = vehicle.FleetId,
                FleetName = vehicle.FleetName,
                DriverId = vehicle.DriverId,
                DriverName = vehicle.DriverName,
                LastServiceDate = vehicle.LastServiceDate,
                NextServiceDate = vehicle.NextServiceDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt
            };
        }
    }
}
