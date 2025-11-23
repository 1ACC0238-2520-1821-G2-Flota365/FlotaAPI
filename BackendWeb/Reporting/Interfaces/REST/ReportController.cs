using BackendWeb.Reporting.Application;
using Microsoft.AspNetCore.Mvc;
using Reporting.Domain;

namespace BackendWeb.Reporting.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<Report>> Generate([FromBody] CreateReportRequest request)
        {
            var report = await _service.Generate(request.Title, request.Type);
            return CreatedAtAction(nameof(GetAll), new { id = report.Id }, report);
        }

        [HttpGet]
        public async Task<ActionResult<List<Report>>> GetAll() => await _service.GetAll();
    }

    public record CreateReportRequest(string Title, string Type);
}
