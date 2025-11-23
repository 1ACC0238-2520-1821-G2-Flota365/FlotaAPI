using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Health
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(AppDbContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Check API health status
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<object>> GetHealth()
        {
            try
            {
                // Test database connection
                var canConnect = await _context.Database.CanConnectAsync();
                
                // Get basic counts
                var vehicleCount = await _context.Vehicles.CountAsync();
                var driverCount = await _context.Drivers.CountAsync();

                return Ok(new
                {
                    status = "healthy",
                    timestamp = DateTime.UtcNow,
                    database = new
                    {
                        connected = canConnect,
                        vehicleCount,
                        driverCount
                    },
                    version = "1.0.0"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new
                {
                    status = "unhealthy",
                    timestamp = DateTime.UtcNow,
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Get detailed system information
        /// </summary>
        [HttpGet("info")]
        public async Task<ActionResult<object>> GetInfo()
        {
            try
            {
                var dbInfo = new
                {
                    CanConnect = await _context.Database.CanConnectAsync(),
                    PendingMigrations = (await _context.Database.GetPendingMigrationsAsync()).Count(),
                    LastMigration = (await _context.Database.GetAppliedMigrationsAsync()).LastOrDefault() ?? "None"
                };

                var counts = new
                {
                    Vehicles = await _context.Vehicles.CountAsync(),
                    Drivers = await _context.Drivers.CountAsync(),
                    Users = await _context.Users.CountAsync(),
                    MaintenanceRecords = await _context.MaintenanceRecords.CountAsync()
                };

                return Ok(new
                {
                    timestamp = DateTime.UtcNow,
                    database = dbInfo,
                    entityCounts = counts,
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Info check failed");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
