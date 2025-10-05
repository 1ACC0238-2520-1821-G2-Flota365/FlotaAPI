using BackendWeb.Management.Application;
using Management.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Management.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService _service;

        public ManagerController(ManagerService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<Manager>> Register([FromBody] CreateManagerRequest request)
        {
            var manager = await _service.Register(request.Name, request.Email);
            return CreatedAtAction(nameof(GetAll), new { id = manager.Id }, manager);
        }

        [HttpGet]
        public async Task<ActionResult<List<Manager>>> GetAll() => await _service.GetAll();
    }

    public record CreateManagerRequest(string Name, string Email);
}
