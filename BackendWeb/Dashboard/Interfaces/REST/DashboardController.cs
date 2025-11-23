using BackendWeb.Dashboard.Application.DTOs;
using BackendWeb.Dashboard.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Dashboard.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        /// <returns>Dashboard statistics including vehicle counts, driver counts, maintenance info, and efficiency metrics</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            try
            {
                var stats = await _service.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard statistics", error = ex.Message });
            }
        }

        /// <summary>
        /// Get active vehicles for dashboard
        /// </summary>
        /// <returns>List of active vehicles with their status and details</returns>
        [HttpGet("active-vehicles")]
        public async Task<ActionResult<List<ActiveVehicleDto>>> GetActiveVehicles()
        {
            try
            {
                var activeVehicles = await _service.GetActiveVehiclesAsync();
                return Ok(activeVehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving active vehicles", error = ex.Message });
            }
        }

        /// <summary>
        /// Get fleet summary for dashboard
        /// </summary>
        /// <returns>Fleet summary including vehicle distribution, efficiency metrics, and trends</returns>
        [HttpGet("fleet-summary")]
        public async Task<ActionResult<FleetSummaryDto>> GetFleetSummary()
        {
            try
            {
                var fleetSummary = await _service.GetFleetSummaryAsync();
                return Ok(fleetSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving fleet summary", error = ex.Message });
            }
        }
    }
}
