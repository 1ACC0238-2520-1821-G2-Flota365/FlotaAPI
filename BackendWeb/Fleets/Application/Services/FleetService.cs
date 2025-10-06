using BackendWeb.Fleets.Application.DTOs;
using BackendWeb.Fleets.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Fleets.Application.Services
{
    public class FleetService
    {
        private readonly IFleetRepository _fleetRepository;
        private readonly AppDbContext _context;

        public FleetService(IFleetRepository fleetRepository, AppDbContext context)
        {
            _fleetRepository = fleetRepository;
            _context = context;
        }

        /// <summary>
        /// Get all fleets with vehicle statistics
        /// </summary>
        public async Task<List<FleetDto>> GetAllFleetsAsync()
        {
            var fleets = await _fleetRepository.GetAllAsync();
            var fleetDtos = new List<FleetDto>();

            foreach (var fleet in fleets)
            {
                var fleetDto = await MapToFleetDtoWithStats(fleet);
                fleetDtos.Add(fleetDto);
            }

            return fleetDtos;
        }

        /// <summary>
        /// Get fleet by ID with vehicle statistics
        /// </summary>
        public async Task<FleetDto?> GetFleetByIdAsync(int id)
        {
            var fleet = await _fleetRepository.GetByIdAsync(id);
            if (fleet == null) return null;

            return await MapToFleetDtoWithStats(fleet);
        }

        /// <summary>
        /// Create a new fleet
        /// </summary>
        public async Task<FleetDto> CreateFleetAsync(CreateFleetRequest request)
        {
            var fleet = new Fleet(request.Name, request.Description, (FleetType)request.Type);
            await _fleetRepository.AddAsync(fleet);

            return await MapToFleetDtoWithStats(fleet);
        }

        /// <summary>
        /// Update an existing fleet
        /// </summary>
        public async Task<FleetDto?> UpdateFleetAsync(int id, UpdateFleetRequest request)
        {
            var fleet = await _fleetRepository.GetByIdAsync(id);
            if (fleet == null) return null;

            fleet.UpdateDetails(request.Name, request.Description, (FleetType)request.Type);
            fleet.UpdateStatus(request.IsActive);

            await _fleetRepository.UpdateAsync(fleet);

            return await MapToFleetDtoWithStats(fleet);
        }

        /// <summary>
        /// Delete a fleet
        /// </summary>
        public async Task<bool> DeleteFleetAsync(int id)
        {
            var exists = await _fleetRepository.ExistsAsync(id);
            if (!exists) return false;

            await _fleetRepository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Update fleet vehicle statistics
        /// </summary>
        public async Task UpdateFleetStatsAsync()
        {
            var fleets = await _fleetRepository.GetAllAsync();

            foreach (var fleet in fleets)
            {
                var vehicleStats = await GetVehicleStatsByFleetName(fleet.Name);
                fleet.UpdateVehicleStats(
                    vehicleStats.TotalVehicles,
                    vehicleStats.ActiveVehicles,
                    vehicleStats.InMaintenanceVehicles
                );

                await _fleetRepository.UpdateAsync(fleet);
            }
        }

        #region Private Methods

        private async Task<FleetDto> MapToFleetDtoWithStats(Fleet fleet)
        {
            // Update vehicle statistics before mapping
            var vehicleStats = await GetVehicleStatsByFleetName(fleet.Name);
            fleet.UpdateVehicleStats(
                vehicleStats.TotalVehicles,
                vehicleStats.ActiveVehicles,
                vehicleStats.InMaintenanceVehicles
            );

            return new FleetDto
            {
                Id = fleet.Id,
                Code = fleet.Code,
                Name = fleet.Name,
                Description = fleet.Description,
                Type = (int)fleet.Type,
                TypeName = fleet.TypeName,
                IsActive = fleet.IsActive,
                VehicleCount = fleet.VehicleCount,
                ActiveVehicles = fleet.ActiveVehicles,
                InMaintenanceVehicles = fleet.InMaintenanceVehicles,
                Performance = fleet.Performance,
                CreatedAt = fleet.CreatedAt,
                UpdatedAt = fleet.UpdatedAt,
                PerformancePercentage = fleet.PerformancePercentage,
                StatusText = fleet.StatusText,
                VehicleUtilization = fleet.VehicleUtilization
            };
        }

        private async Task<(int TotalVehicles, int ActiveVehicles, int InMaintenanceVehicles)> GetVehicleStatsByFleetName(string fleetName)
        {
            try
            {
                var totalVehicles = await _context.Vehicles
                    .Where(v => v.FleetName == fleetName)
                    .CountAsync();

                var activeVehicles = await _context.Vehicles
                    .Where(v => v.FleetName == fleetName && v.Status == 1) // 1 = Active
                    .CountAsync();

                // Count vehicles in maintenance by checking maintenance records
                var inMaintenanceVehicles = 0;
                if (_context.MaintenanceRecords != null)
                {
                    var vehiclesInMaintenance = await _context.MaintenanceRecords
                        .Where(m => m.Status == BackendWeb.Maintenance.Domain.MaintenanceStatus.InProgress)
                        .Join(_context.Vehicles,
                            m => m.VehicleId,
                            v => v.Id,
                            (m, v) => new { v.FleetName })
                        .Where(x => x.FleetName == fleetName)
                        .CountAsync();

                    inMaintenanceVehicles = vehiclesInMaintenance;
                }

                return (totalVehicles, activeVehicles, inMaintenanceVehicles);
            }
            catch
            {
                // Return default values if there's an issue accessing vehicle data
                return (0, 0, 0);
            }
        }

        #endregion
    }
}
