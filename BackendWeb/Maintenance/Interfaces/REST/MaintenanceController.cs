using BackendWeb.Maintenance.Application.DTOs;
using BackendWeb.Maintenance.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWeb.Maintenance.Interfaces.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceService _service;

        public MaintenanceController(MaintenanceService service)
        {
            _service = service;
        }

        // Maintenance Records Endpoints

        /// <summary>
        /// Get all maintenance records
        /// </summary>
        [HttpGet("records")]
        public async Task<ActionResult<List<MaintenanceRecordDto>>> GetMaintenanceRecords()
        {
            try
            {
                var records = await _service.GetAllMaintenanceRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving maintenance records", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new maintenance record
        /// </summary>
        [HttpPost("records")]
        public async Task<ActionResult<MaintenanceRecordDto>> CreateMaintenanceRecord([FromBody] CreateMaintenanceRecordRequest request)
        {
            try
            {
                var record = await _service.CreateMaintenanceRecord(request);
                return Ok(record);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating maintenance record", error = ex.Message });
            }
        }

        /// <summary>
        /// Get maintenance record by ID
        /// </summary>
        [HttpGet("records/{id}")]
        public async Task<ActionResult<MaintenanceRecordDto>> GetMaintenanceRecord(int id)
        {
            try
            {
                var record = await _service.GetMaintenanceRecordById(id);
                if (record == null)
                    return NotFound(new { message = "Maintenance record not found" });

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving maintenance record", error = ex.Message });
            }
        }

        /// <summary>
        /// Update maintenance record
        /// </summary>
        [HttpPut("records/{id}")]
        public async Task<ActionResult<MaintenanceRecordDto>> UpdateMaintenanceRecord(int id, [FromBody] UpdateMaintenanceRecordRequest request)
        {
            try
            {
                var record = await _service.UpdateMaintenanceRecord(id, request);
                if (record == null)
                    return NotFound(new { message = "Maintenance record not found" });

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating maintenance record", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete maintenance record
        /// </summary>
        [HttpDelete("records/{id}")]
        public async Task<ActionResult> DeleteMaintenanceRecord(int id)
        {
            try
            {
                await _service.DeleteMaintenanceRecord(id);
                return Ok(new { message = "Maintenance record deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting maintenance record", error = ex.Message });
            }
        }

        /// <summary>
        /// Get maintenance records by vehicle ID
        /// </summary>
        [HttpGet("records/vehicle/{vehicleId}")]
        public async Task<ActionResult<List<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicle(int vehicleId)
        {
            try
            {
                var records = await _service.GetMaintenanceRecordsByVehicle(vehicleId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving maintenance records", error = ex.Message });
            }
        }

        /// <summary>
        /// Get overdue maintenance records
        /// </summary>
        [HttpGet("records/overdue")]
        public async Task<ActionResult<List<MaintenanceRecordDto>>> GetOverdueMaintenanceRecords()
        {
            try
            {
                var records = await _service.GetOverdueMaintenanceRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving overdue maintenance records", error = ex.Message });
            }
        }

        // Service Records Endpoints

        /// <summary>
        /// Get all service records
        /// </summary>
        [HttpGet("services")]
        public async Task<ActionResult<List<ServiceRecordDto>>> GetServiceRecords()
        {
            try
            {
                var records = await _service.GetAllServiceRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving service records", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new service record
        /// </summary>
        [HttpPost("services")]
        public async Task<ActionResult<ServiceRecordDto>> CreateServiceRecord([FromBody] CreateServiceRecordRequest request)
        {
            try
            {
                var record = await _service.CreateServiceRecord(request);
                return Ok(record);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating service record", error = ex.Message });
            }
        }

        /// <summary>
        /// Get service record by ID
        /// </summary>
        [HttpGet("services/{id}")]
        public async Task<ActionResult<ServiceRecordDto>> GetServiceRecord(int id)
        {
            try
            {
                var record = await _service.GetServiceRecordById(id);
                if (record == null)
                    return NotFound(new { message = "Service record not found" });

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving service record", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete service record
        /// </summary>
        [HttpDelete("services/{id}")]
        public async Task<ActionResult> DeleteServiceRecord(int id)
        {
            try
            {
                await _service.DeleteServiceRecord(id);
                return Ok(new { message = "Service record deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting service record", error = ex.Message });
            }
        }

        /// <summary>
        /// Get service records by vehicle ID
        /// </summary>
        [HttpGet("services/vehicle/{vehicleId}")]
        public async Task<ActionResult<List<ServiceRecordDto>>> GetServiceRecordsByVehicle(int vehicleId)
        {
            try
            {
                var records = await _service.GetServiceRecordsByVehicle(vehicleId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving service records", error = ex.Message });
            }
        }
    }
}
