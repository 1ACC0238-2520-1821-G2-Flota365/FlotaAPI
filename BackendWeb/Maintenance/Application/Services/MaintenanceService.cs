using BackendWeb.Maintenance.Domain;
using BackendWeb.Maintenance.Application.DTOs;
using BackendWeb.FleetManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Maintenance.Application.Services
{
    public class MaintenanceService
    {
        private readonly IMaintenanceRecordRepository _maintenanceRepository;
        private readonly IServiceRecordRepository _serviceRepository;
        private readonly AppDbContext _context;

        public MaintenanceService(
            IMaintenanceRecordRepository maintenanceRepository,
            IServiceRecordRepository serviceRepository,
            AppDbContext context)
        {
            _maintenanceRepository = maintenanceRepository;
            _serviceRepository = serviceRepository;
            _context = context;
        }

        // Maintenance Records
        public async Task<MaintenanceRecordDto> CreateMaintenanceRecord(CreateMaintenanceRecordRequest request)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.VehicleId);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found.");

            var record = new MaintenanceRecord(
                request.VehicleId,
                request.Description,
                (MaintenanceType)request.Type,
                request.Cost,
                request.ScheduledDate,
                request.Notes
            );

            record.SetVehicleInfo(vehicle.LicensePlate, $"{vehicle.Brand} {vehicle.Model}");

            await _maintenanceRepository.AddAsync(record);
            return MapToMaintenanceRecordDto(record);
        }

        public async Task<List<MaintenanceRecordDto>> GetAllMaintenanceRecords()
        {
            var records = await _maintenanceRepository.GetAllAsync();
            return records.Select(MapToMaintenanceRecordDto).ToList();
        }

        public async Task<MaintenanceRecordDto?> GetMaintenanceRecordById(int id)
        {
            var record = await _maintenanceRepository.GetByIdAsync(id);
            return record == null ? null : MapToMaintenanceRecordDto(record);
        }

        public async Task<List<MaintenanceRecordDto>> GetMaintenanceRecordsByVehicle(int vehicleId)
        {
            var records = await _maintenanceRepository.GetByVehicleIdAsync(vehicleId);
            return records.Select(MapToMaintenanceRecordDto).ToList();
        }

        public async Task<List<MaintenanceRecordDto>> GetOverdueMaintenanceRecords()
        {
            var records = await _maintenanceRepository.GetOverdueAsync();
            return records.Select(MapToMaintenanceRecordDto).ToList();
        }

        public async Task<MaintenanceRecordDto?> UpdateMaintenanceRecord(int id, UpdateMaintenanceRecordRequest request)
        {
            var record = await _maintenanceRepository.GetByIdAsync(id);
            if (record == null) return null;

            record.UpdateDetails(
                request.Description,
                (MaintenanceType)request.Type,
                request.Cost,
                request.ScheduledDate,
                request.Notes
            );

            if (request.CompletedDate.HasValue || request.Status != (int)record.Status)
            {
                record.UpdateStatus((MaintenanceStatus)request.Status, request.CompletedDate);
            }

            await _maintenanceRepository.UpdateAsync(record);
            return MapToMaintenanceRecordDto(record);
        }

        public async Task DeleteMaintenanceRecord(int id)
        {
            await _maintenanceRepository.DeleteAsync(id);
        }

        // Service Records
        public async Task<ServiceRecordDto> CreateServiceRecord(CreateServiceRecordRequest request)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.VehicleId);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found.");

            var record = new ServiceRecord(
                request.VehicleId,
                request.ServiceType,
                request.Description,
                request.Cost,
                request.ServiceDate,
                request.MileageAtService,
                request.ServiceProvider
            );

            record.SetVehicleInfo(vehicle.LicensePlate);

            await _serviceRepository.AddAsync(record);
            return MapToServiceRecordDto(record);
        }

        public async Task<List<ServiceRecordDto>> GetAllServiceRecords()
        {
            var records = await _serviceRepository.GetAllAsync();
            return records.Select(MapToServiceRecordDto).ToList();
        }

        public async Task<ServiceRecordDto?> GetServiceRecordById(int id)
        {
            var record = await _serviceRepository.GetByIdAsync(id);
            return record == null ? null : MapToServiceRecordDto(record);
        }

        public async Task<List<ServiceRecordDto>> GetServiceRecordsByVehicle(int vehicleId)
        {
            var records = await _serviceRepository.GetByVehicleIdAsync(vehicleId);
            return records.Select(MapToServiceRecordDto).ToList();
        }

        public async Task DeleteServiceRecord(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        // Mapping methods
        private MaintenanceRecordDto MapToMaintenanceRecordDto(MaintenanceRecord record)
        {
            return new MaintenanceRecordDto
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                VehicleLicensePlate = record.VehicleLicensePlate,
                VehicleModel = record.VehicleModel,
                Description = record.Description,
                Type = (int)record.Type,
                TypeName = record.TypeName,
                Cost = record.Cost,
                ScheduledDate = record.ScheduledDate,
                CompletedDate = record.CompletedDate,
                Status = (int)record.Status,
                StatusName = record.StatusName,
                Notes = record.Notes,
                CreatedAt = record.CreatedAt,
                UpdatedAt = record.UpdatedAt,
                IsOverdue = record.IsOverdue,
                DaysOverdue = record.DaysOverdue
            };
        }

        private ServiceRecordDto MapToServiceRecordDto(ServiceRecord record)
        {
            return new ServiceRecordDto
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                VehicleLicensePlate = record.VehicleLicensePlate,
                ServiceType = record.ServiceType,
                Description = record.Description,
                Cost = record.Cost,
                ServiceDate = record.ServiceDate,
                MileageAtService = record.MileageAtService,
                ServiceProvider = record.ServiceProvider,
                CreatedAt = record.CreatedAt
            };
        }
    }
}
