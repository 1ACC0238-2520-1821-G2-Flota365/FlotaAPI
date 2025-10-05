using BackendWeb.Fleets.Application.DTOs;
using BackendWeb.Fleets.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Fleets.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class FleetsController : ControllerBase
    {
        private readonly FleetService _service;

        public FleetsController(FleetService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all fleets
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<FleetDto>>> GetFleets()
        {
            try
            {
                var fleets = await _service.GetAllFleetsAsync();
                return Ok(fleets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving fleets", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new fleet
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FleetDto>> CreateFleet([FromBody] CreateFleetRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(new { message = "Fleet name is required" });
                }

                var fleet = await _service.CreateFleetAsync(request);
                return Ok(fleet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating fleet", error = ex.Message });
            }
        }

        /// <summary>
        /// Get fleet by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FleetDto>> GetFleet(int id)
        {
            try
            {
                var fleet = await _service.GetFleetByIdAsync(id);
                if (fleet == null)
                    return NotFound(new { message = "Fleet not found" });

                return Ok(fleet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving fleet", error = ex.Message });
            }
        }

        /// <summary>
        /// Update fleet
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<FleetDto>> UpdateFleet(int id, [FromBody] UpdateFleetRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(new { message = "Fleet name is required" });
                }

                var fleet = await _service.UpdateFleetAsync(id, request);
                if (fleet == null)
                    return NotFound(new { message = "Fleet not found" });

                return Ok(fleet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating fleet", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete fleet
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFleet(int id)
        {
            try
            {
                var success = await _service.DeleteFleetAsync(id);
                if (!success)
                    return NotFound(new { message = "Fleet not found" });

                return Ok(new { message = "Fleet deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting fleet", error = ex.Message });
            }
        }
    }
}
