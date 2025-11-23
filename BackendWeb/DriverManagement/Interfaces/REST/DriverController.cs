using BackendWeb.DriverManagement.Application.DTOs;
using DriverManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DriverManagement.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly DriverService _service;

        public DriverController(DriverService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<DriverDto>> Register([FromBody] CreateDriverDto dto)
        {
            var driver = await _service.RegisterDriverAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driver);
        }

        [HttpGet]
        public async Task<ActionResult<List<DriverDto>>> GetAll()
        {
            var drivers = await _service.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverDto>> GetById(int id)
        {
            var driver = await _service.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound(new { message = "Conductor no encontrado." });
            }
            return Ok(driver);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DriverDto>> UpdateDriver(int id, [FromBody] UpdateDriverDto updateDto)
        {
            var driver = await _service.UpdateDriverAsync(id, updateDto);
            if (driver == null)
            {
                return NotFound(new { message = "Conductor no encontrado." });
            }
            return Ok(driver);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDriver(int id)
        {
            var result = await _service.DeleteDriverAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Conductor no encontrado." });
            }
            return Ok(new { message = "Conductor desactivado exitosamente." });
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DriverStatsDto>> GetStats()
        {
            var stats = await _service.GetDriverStatsAsync();
            return Ok(stats);
        }
    }
}
