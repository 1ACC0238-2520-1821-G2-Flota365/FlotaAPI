using BackendWeb.FleetManagement.Application.DTOs;
using FleetManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _service;

        public VehicleController(VehicleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Register([FromBody] CreateVehicleDto dto)
        {
            var vehicle = await _service.RegisterVehicleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpGet]
        public async Task<ActionResult<List<VehicleDto>>> GetAll()
        {
            var vehicles = await _service.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(int id)
        {
            var vehicle = await _service.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehículo no encontrado." });
            }
            return Ok(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, [FromBody] UpdateVehicleDto updateDto)
        {
            var vehicle = await _service.UpdateVehicleAsync(id, updateDto);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehículo no encontrado." });
            }
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var result = await _service.DeleteVehicleAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Vehículo no encontrado." });
            }
            return Ok(new { message = "Vehículo desactivado exitosamente." });
        }
    }
}
