using Microsoft.EntityFrameworkCore;
using BackendWeb.Assignments.Domain;
using BackendWeb.DriverManagement.Domain;
using BackendWeb.FleetManagement.Domain;
using BackendWeb.Maintenance.Domain;
using Reporting.Domain;
using Management.Domain;
using BackendWeb.Auth.Domain;

namespace Shared.Persistence.EFC
{
    public class AppDbContext : DbContext
    {
        // Authentication
        public DbSet<User> Users => Set<User>();

        // Fleet Management
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        // Driver Management
        public DbSet<Driver> Drivers => Set<Driver>();

        // Assignments
        public DbSet<Assignment> Assignments => Set<Assignment>();

        // Maintenance
        public DbSet<MaintenanceOrder> MaintenanceOrders => Set<MaintenanceOrder>();
        public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
        public DbSet<ServiceRecord> ServiceRecords => Set<ServiceRecord>();

        // Reporting
        public DbSet<Report> Reports => Set<Report>();

        // Management
        public DbSet<Manager> Managers => Set<Manager>();

        // Fleets
        public DbSet<BackendWeb.Fleets.Domain.Fleet> Fleets => Set<BackendWeb.Fleets.Domain.Fleet>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Auth
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Fleet
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).ValueGeneratedOnAdd();
                entity.Property(v => v.LicensePlate).IsRequired().HasMaxLength(20);
                entity.Property(v => v.Brand).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Model).IsRequired().HasMaxLength(50);
                entity.Property(v => v.StatusName).IsRequired().HasMaxLength(20);
                entity.Property(v => v.FleetName).IsRequired().HasMaxLength(100);
                entity.Property(v => v.DriverName).HasMaxLength(100);
                entity.Property(v => v.CreatedAt).IsRequired();
                entity.Property(v => v.UpdatedAt).IsRequired();
                
                entity.HasIndex(v => v.LicensePlate).IsUnique();
            });

            // Driver
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).ValueGeneratedOnAdd();
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                entity.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(d => d.LastName).IsRequired().HasMaxLength(50);
                entity.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(30);
                entity.Property(d => d.Phone).IsRequired().HasMaxLength(20);
                entity.Property(d => d.Email).IsRequired().HasMaxLength(100);
                entity.Property(d => d.StatusName).IsRequired().HasMaxLength(20);
                entity.Property(d => d.AssignedVehicle).HasMaxLength(100);
                entity.Property(d => d.CreatedAt).IsRequired();
                entity.Property(d => d.UpdatedAt).IsRequired();
                
                entity.HasIndex(d => d.Code).IsUnique();
                entity.HasIndex(d => d.LicenseNumber).IsUnique();
                entity.HasIndex(d => d.Email).IsUnique();
            });

            // Assignment
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Route).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Status).IsRequired().HasMaxLength(20);
            });

            // Maintenance
            modelBuilder.Entity<MaintenanceOrder>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Type).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Status).IsRequired().HasMaxLength(20);
            });

            // MaintenanceRecord
            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.Property(m => m.VehicleId).IsRequired();
                entity.Property(m => m.VehicleLicensePlate).HasMaxLength(20);
                entity.Property(m => m.VehicleModel).HasMaxLength(100);
                entity.Property(m => m.Description).IsRequired().HasMaxLength(500);
                entity.Property(m => m.Type).IsRequired().HasConversion<int>();
                entity.Property(m => m.TypeName).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Cost).HasColumnType("decimal(10,2)");
                entity.Property(m => m.ScheduledDate).IsRequired();
                entity.Property(m => m.CompletedDate);
                entity.Property(m => m.Status).IsRequired().HasConversion<int>();
                entity.Property(m => m.StatusName).IsRequired().HasMaxLength(20);
                entity.Property(m => m.Notes).HasMaxLength(1000);
                entity.Property(m => m.CreatedAt).IsRequired();
                entity.Property(m => m.UpdatedAt).IsRequired();
                
                // Navigation properties can be added if needed
                // entity.HasOne<Vehicle>().WithMany().HasForeignKey(m => m.VehicleId);
            });

            // ServiceRecord
            modelBuilder.Entity<ServiceRecord>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.Property(s => s.VehicleId).IsRequired();
                entity.Property(s => s.VehicleLicensePlate).HasMaxLength(20);
                entity.Property(s => s.ServiceType).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Description).IsRequired().HasMaxLength(500);
                entity.Property(s => s.Cost).HasColumnType("decimal(10,2)");
                entity.Property(s => s.ServiceDate).IsRequired();
                entity.Property(s => s.MileageAtService).IsRequired();
                entity.Property(s => s.ServiceProvider).IsRequired().HasMaxLength(200);
                entity.Property(s => s.CreatedAt).IsRequired();
                
                // Navigation properties can be added if needed
                // entity.HasOne<Vehicle>().WithMany().HasForeignKey(s => s.VehicleId);
            });

            // Report
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Title).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Type).IsRequired().HasMaxLength(50);
            });

            // Manager
            modelBuilder.Entity<Manager>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                entity.Property(m => m.Email).IsRequired().HasMaxLength(100);
            });

            // Fleet
            modelBuilder.Entity<BackendWeb.Fleets.Domain.Fleet>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).ValueGeneratedOnAdd();
                entity.Property(f => f.Code).IsRequired().HasMaxLength(50);
                entity.Property(f => f.Name).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Description).HasMaxLength(500);
                entity.Property(f => f.Type).IsRequired().HasConversion<int>();
                entity.Property(f => f.TypeName).IsRequired().HasMaxLength(50);
                entity.Property(f => f.IsActive).IsRequired();
                entity.Property(f => f.VehicleCount).HasDefaultValue(0);
                entity.Property(f => f.ActiveVehicles).HasDefaultValue(0);
                entity.Property(f => f.InMaintenanceVehicles).HasDefaultValue(0);
                entity.Property(f => f.Performance).HasColumnType("decimal(5,4)").HasDefaultValue(0.0m);
                entity.Property(f => f.CreatedAt).IsRequired();
                entity.Property(f => f.UpdatedAt).IsRequired();
                
                entity.HasIndex(f => f.Code).IsUnique();
                entity.HasIndex(f => f.Name).IsUnique();
            });
        }
    }
}
