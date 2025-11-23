using Assignments.Application;
using BackendWeb.Assignments.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Assignments.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _service;

        public AssignmentController(AssignmentService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<Assignment>> Create([FromBody] CreateAssignmentRequest request)
        {
            var assignment = await _service.Create(request.VehicleId, request.DriverId, request.Route);
            return CreatedAtAction(nameof(GetAll), new { id = assignment.Id }, assignment);
        }

        [HttpGet]
        public async Task<ActionResult<List<Assignment>>> GetAll() => await _service.GetAll();

        [HttpPut("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            await _service.Start(id);
            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            await _service.Complete(id);
            return NoContent();
        }
    }

    public record CreateAssignmentRequest(int VehicleId, int DriverId, string Route);
}
