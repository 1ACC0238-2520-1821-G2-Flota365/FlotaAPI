using BackendWeb.Dashboard.Application.DTOs;
using BackendWeb.FleetManagement.Domain;
using BackendWeb.DriverManagement.Domain;
using BackendWeb.Maintenance.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Dashboard.Application.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                // Get current statistics
                var totalVehicles = await _context.Vehicles.CountAsync();
                var activeDrivers = await _context.Drivers
                    .Where(d => d.Status == 1) // 1 = Active
                    .CountAsync();

                // Handle maintenance records safely - they might not exist yet
                var vehiclesInMaintenance = 0;
                var vehiclesDueForService = 0;
                var alertsCount = 0;
                
                try
                {
                    vehiclesInMaintenance = await _context.MaintenanceRecords
                        .Where(m => m.Status == MaintenanceStatus.InProgress)
                        .Select(m => m.VehicleId)
                        .Distinct()
                        .CountAsync();

                    // Get vehicles due for service (example: maintenance scheduled within next 30 days)
                    var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);
                    vehiclesDueForService = await _context.MaintenanceRecords
                        .Where(m => m.Status == MaintenanceStatus.Scheduled && 
                                   m.ScheduledDate <= thirtyDaysFromNow)
                        .Select(m => m.VehicleId)
                        .Distinct()
                        .CountAsync();

                    // Mock alerts count (could be based on overdue maintenance, etc.)
                    alertsCount = await _context.MaintenanceRecords
                        .Where(m => m.IsOverdue)
                        .CountAsync();
                }
                catch (Exception ex)
                {
                    // Log but don't fail - maintenance records might not exist
                    Console.WriteLine($"Warning: Could not fetch maintenance data: {ex.Message}");
                }

                var totalFleets = await _context.Vehicles
                    .Where(v => !string.IsNullOrEmpty(v.FleetName))
                    .Select(v => v.FleetName)
                    .Distinct()
                    .CountAsync();

                // Calculate fleet efficiency (example calculation based on active vehicles)
                var activeVehicles = await _context.Vehicles
                    .Where(v => v.Status == 1) // 1 = Active
                    .CountAsync();
                
                var fleetEfficiency = totalVehicles > 0 ? 
                    Math.Round((decimal)activeVehicles / totalVehicles * 100, 2) : 0;

                // Calculate average vehicle age (mock calculation)
                var averageVehicleAge = await CalculateAverageVehicleAge();

                return new DashboardStatsDto
                {
                    TotalVehicles = totalVehicles,
                    ActiveDrivers = activeDrivers,
                    VehiclesInMaintenance = vehiclesInMaintenance,
                    FleetEfficiency = fleetEfficiency,
                    TotalVehiclesChange = CalculateChange(totalVehicles, totalVehicles), // Mock change
                    ActiveDriversChange = CalculateChange(activeDrivers, activeDrivers), // Mock change
                    MaintenanceChange = CalculateChange(vehiclesInMaintenance, vehiclesInMaintenance), // Mock change
                    EfficiencyChange = CalculateChange((int)fleetEfficiency, (int)fleetEfficiency), // Mock change
                    LastUpdated = DateTime.UtcNow,
                    TotalFleets = totalFleets,
                    AlertsCount = alertsCount,
                    AverageVehicleAge = averageVehicleAge,
                    VehiclesDueForService = vehiclesDueForService
                };
            }
            catch (Exception ex)
            {
                // Return default values if something goes wrong
                Console.WriteLine($"Error in GetDashboardStatsAsync: {ex.Message}");
                return new DashboardStatsDto
                {
                    TotalVehicles = 0,
                    ActiveDrivers = 0,
                    VehiclesInMaintenance = 0,
                    FleetEfficiency = 0,
                    TotalVehiclesChange = "0%",
                    ActiveDriversChange = "0%",
                    MaintenanceChange = "0%",
                    EfficiencyChange = "0%",
                    LastUpdated = DateTime.UtcNow,
                    TotalFleets = 0,
                    AlertsCount = 0,
                    AverageVehicleAge = 0,
                    VehiclesDueForService = 0
                };
            }
        }

        /// <summary>
        /// Get active vehicles for dashboard
        /// </summary>
        public async Task<List<ActiveVehicleDto>> GetActiveVehiclesAsync()
        {
            try
            {
                var activeVehicles = await _context.Vehicles
                    .Where(v => v.Status == 1) // 1 = Active
                    .ToListAsync();

                var result = activeVehicles.Select(v => new ActiveVehicleDto
                {
                    Id = v.Id,
                    LicensePlate = v.LicensePlate ?? "N/A",
                    Model = $"{v.Brand ?? ""} {v.Model ?? ""}".Trim(),
                    DriverName = !string.IsNullOrEmpty(v.DriverName) ? v.DriverName : "No driver assigned",
                    Status = v.Status,
                    StatusName = v.StatusName ?? "Active",
                    FleetName = v.FleetName ?? "Sin flota",
                    LastUpdate = v.UpdatedAt,
                    StatusColor = GetStatusColor(v.Status)
                }).OrderBy(v => v.LicensePlate).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveVehiclesAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Return empty list instead of throwing
                return new List<ActiveVehicleDto>();
            }
        }

        /// <summary>
        /// Get fleet summary for dashboard
        /// </summary>
        public async Task<FleetSummaryDto> GetFleetSummaryAsync()
        {
            try
            {
                // Group vehicles by fleet type
                var fleetGroups = await _context.Vehicles
                    .Where(v => !string.IsNullOrEmpty(v.FleetName))
                    .GroupBy(v => v.FleetName)
                    .Select(g => new { FleetName = g.Key, Count = g.Count() })
                    .ToListAsync();

                var totalFleets = fleetGroups.Count;

                // Categorize fleets (this is a simplified example)
                var primaryFleetVehicles = fleetGroups
                    .Where(f => f.FleetName != null && (f.FleetName.ToLower().Contains("primary") || f.FleetName.ToLower().Contains("principal")))
                    .Sum(f => f.Count);

                var secondaryFleetVehicles = fleetGroups
                    .Where(f => f.FleetName != null && (f.FleetName.ToLower().Contains("secondary") || f.FleetName.ToLower().Contains("secundario")))
                    .Sum(f => f.Count);

                var externalFleetVehicles = fleetGroups
                    .Where(f => f.FleetName != null && (f.FleetName.ToLower().Contains("external") || f.FleetName.ToLower().Contains("externo")))
                    .Sum(f => f.Count);

                // Calculate efficiency for each fleet type (mock calculation)
                var primaryFleetEfficiency = await CalculateFleetEfficiency("primary");
                var secondaryFleetEfficiency = await CalculateFleetEfficiency("secondary");
                var externalFleetEfficiency = await CalculateFleetEfficiency("external");

                var overallEfficiency = totalFleets > 0 ? (primaryFleetEfficiency + secondaryFleetEfficiency + externalFleetEfficiency) / 3 : 0;

                return new FleetSummaryDto
                {
                    TotalFleets = totalFleets,
                    PrimaryFleetVehicles = primaryFleetVehicles,
                    SecondaryFleetVehicles = secondaryFleetVehicles,
                    ExternalFleetVehicles = externalFleetVehicles,
                    PrimaryFleetEfficiency = primaryFleetEfficiency,
                    SecondaryFleetEfficiency = secondaryFleetEfficiency,
                    ExternalFleetEfficiency = externalFleetEfficiency,
                    OverallEfficiency = overallEfficiency,
                    PrimaryFleetTrend = GetTrendIndicator(primaryFleetEfficiency),
                    SecondaryFleetTrend = GetTrendIndicator(secondaryFleetEfficiency),
                    ExternalFleetTrend = GetTrendIndicator(externalFleetEfficiency)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFleetSummaryAsync: {ex.Message}");
                // Return default values if something goes wrong
                return new FleetSummaryDto
                {
                    TotalFleets = 0,
                    PrimaryFleetVehicles = 0,
                    SecondaryFleetVehicles = 0,
                    ExternalFleetVehicles = 0,
                    PrimaryFleetEfficiency = 0,
                    SecondaryFleetEfficiency = 0,
                    ExternalFleetEfficiency = 0,
                    OverallEfficiency = 0,
                    PrimaryFleetTrend = "needs_improvement",
                    SecondaryFleetTrend = "needs_improvement",
                    ExternalFleetTrend = "needs_improvement"
                };
            }
        }

        #region Private Helper Methods

        private async Task<decimal> CalculateAverageVehicleAge()
        {
            // Mock calculation - in real scenario, you'd have vehicle manufacturing/purchase dates
            var vehicles = await _context.Vehicles.ToListAsync();
            if (!vehicles.Any()) return 0;

            // Mock: assume vehicles have been in service for random periods
            var random = new Random();
            var totalAge = vehicles.Sum(v => random.Next(1, 10)); // 1-10 years
            return Math.Round((decimal)totalAge / vehicles.Count, 1);
        }

        private async Task<decimal> CalculateFleetEfficiency(string fleetType)
        {
            try
            {
                var fleetVehicles = await _context.Vehicles
                    .Where(v => !string.IsNullOrEmpty(v.FleetName) && v.FleetName.ToLower().Contains(fleetType.ToLower()))
                    .CountAsync();

                if (fleetVehicles == 0) return 0;

                var activeFleetVehicles = await _context.Vehicles
                    .Where(v => !string.IsNullOrEmpty(v.FleetName) && 
                               v.FleetName.ToLower().Contains(fleetType.ToLower()) && 
                               v.Status == 1) // 1 = Active
                    .CountAsync();

                return Math.Round((decimal)activeFleetVehicles / fleetVehicles * 100, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CalculateFleetEfficiency for {fleetType}: {ex.Message}");
                return 0;
            }
        }

        private string GetStatusColor(int status)
        {
            return status switch
            {
                1 => "green",    // Active
                2 => "orange",   // In Maintenance
                3 => "red",      // Out of Service
                4 => "blue",     // Reserved
                _ => "gray"
            };
        }

        private string CalculateChange(int current, int previous)
        {
            if (previous == 0) return current > 0 ? "+100%" : "0%";
            
            var changePercent = Math.Round(((decimal)(current - previous) / previous) * 100, 1);
            return changePercent > 0 ? $"+{changePercent}%" : $"{changePercent}%";
        }

        private string GetTrendIndicator(decimal efficiency)
        {
            return efficiency switch
            {
                >= 90 => "excellent",
                >= 75 => "good",
                >= 60 => "average",
                _ => "needs_improvement"
            };
        }

        #endregion
    }
}
