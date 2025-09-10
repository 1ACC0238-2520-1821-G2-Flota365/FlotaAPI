using FleetManagement.Application.Services;
using FleetManagement.Infrastructure.Repositories;

using DriverManagement.Application.Services;
using DriverManagement.Infrastructure.Repositories;

using Assignments.Application;
using Maintenance.Infrastructure.Repositories;
using BackendWeb.Maintenance.Infrastructure.Repositories;
using Reporting.Domain;
using Management.Domain;

using Shared.Persistence.EFC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BackendWeb.Assignments.Domain;
using BackendWeb.Assignments.Infrastructure.Repositories;
using BackendWeb.DriverManagement.Domain;
using BackendWeb.FleetManagement.Domain;
using BackendWeb.Maintenance.Application.Services;
using BackendWeb.Maintenance.Domain;
using BackendWeb.Management.Application;
using BackendWeb.Management.Infrastructure.Repositories;
using BackendWeb.Reporting.Application;
using BackendWeb.Reporting.Infrastructure.Repositories;
using BackendWeb.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackendWeb.Auth.Domain;
using BackendWeb.Auth.Infrastructure.Repositories;
using BackendWeb.Auth.Application;
using BackendWeb.Dashboard.Application.Services;
using BackendWeb.Fleets.Domain;
using BackendWeb.Fleets.Application.Services;
using BackendWeb.Fleets.Infrastructure.Repositories;
using BCrypt.Net;
using MySql.Data.MySqlClient;


var builder = WebApplication.CreateBuilder(args);

// JWT Config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Database (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));


// Fleet
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<VehicleService>();

// Driver
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<DriverService>();

// Assignments
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<AssignmentService>();

// Maintenance
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddScoped<IMaintenanceRecordRepository>(provider => provider.GetService<IMaintenanceRepository>() as IMaintenanceRecordRepository);
builder.Services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
builder.Services.AddScoped<MaintenanceService>();

// Reporting
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();

// Management
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<ManagerService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8082", "http://localhost:8080", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorio + servicio de autenticación
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

// Dashboard
builder.Services.AddScoped<DashboardService>();

// Fleets
builder.Services.AddScoped<IFleetRepository, FleetRepository>();
builder.Services.AddScoped<FleetService>();

var app = builder.Build();

// Apply migrations and create DB if not exists
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Applying database migrations...");

        // Aplica migraciones y crea la base si no existe
        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Database migrations applied successfully.");

        // Semilla de datos
        await SeedTestDataAsync(dbContext, logger);
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Error during database setup: {Message}", ex.Message);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

// Enable CORS
app.UseCors("AllowFrontend");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

// Seed test data method
static async Task SeedTestDataAsync(AppDbContext context, ILogger logger)
{
    try
    {
        logger.LogInformation("Checking if test data seeding is needed...");
        
        var vehicleCount = await context.Vehicles.CountAsync();
        var driverCount = await context.Drivers.CountAsync();
        
        logger.LogInformation($"Current data: {vehicleCount} vehicles, {driverCount} drivers");
        
        // Only seed if database is empty
        if (vehicleCount == 0 && driverCount == 0)
        {
            logger.LogInformation("Database is empty. Seeding with test data...");
            
            // Add sample vehicles
            var sampleVehicles = new[]
            {
                new BackendWeb.FleetManagement.Domain.Vehicle("ABC123", "Toyota", "Hiace", 2020, 15000, 1, "Flota Principal"),
                new BackendWeb.FleetManagement.Domain.Vehicle("XYZ789", "Ford", "Transit", 2019, 25000, 1, "Flota Principal"),
                new BackendWeb.FleetManagement.Domain.Vehicle("DEF456", "Chevrolet", "Spark", 2021, 8000, 2, "Flota Secundaria")
            };
            
            context.Vehicles.AddRange(sampleVehicles);
            logger.LogInformation($"Added {sampleVehicles.Length} sample vehicles.");
            
            // Add sample drivers
            var sampleDrivers = new[]
            {
                new BackendWeb.DriverManagement.Domain.Driver("DRV001", "Juan", "Pérez", "L12345678", 
                    DateTime.UtcNow.AddYears(2), "+51987654321", "juan@email.com", 5),
                new BackendWeb.DriverManagement.Domain.Driver("DRV002", "María", "García", "L87654321", 
                    DateTime.UtcNow.AddYears(3), "+51123456789", "maria@email.com", 8)
            };
            
            context.Drivers.AddRange(sampleDrivers);
            logger.LogInformation($"Added {sampleDrivers.Length} sample drivers.");
            
            // Add sample user for authentication
            var hasher = new BCrypt.Net.BCrypt();
            var sampleUser = new BackendWeb.Auth.Domain.User(
                "admin@flota365.com", 
                BCrypt.Net.BCrypt.HashPassword("admin123"), 
                "Admin", 
                "System", 
                "Admin"
            );
            
            context.Users.Add(sampleUser);
            logger.LogInformation("Added sample admin user (email: admin@flota365.com, password: admin123).");
            
            await context.SaveChangesAsync();
            logger.LogInformation("Test data seeded successfully!");
        }
        else
        {
            logger.LogInformation("Database already contains data. Skipping seeding.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occurred while seeding test data: {ErrorMessage}", ex.Message);
        // Don't throw - seeding is not critical for application startup
    }
}

// Alternative database creation method
static async Task CreateDatabaseManuallyAsync(AppDbContext context, ILogger logger)
{
    try
    {
        // Try to create database using SQL commands
        var connectionString = context.Database.GetConnectionString();

        if (string.IsNullOrEmpty(connectionString))
        {
            logger.LogError("❌ No se encontró la cadena de conexión en la configuración.");
            return;
        }
        logger.LogInformation(
            $"Attempting to create database with connection: {connectionString?.Substring(0, Math.Min(50, connectionString.Length))}..."
        );
        // Extract database name from connection string
        var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;
        var serverConnectionString = connectionString?.Replace($"database={databaseName}", "database=information_schema");
        
        // Create database if it doesn't exist
        using (var connection = new MySql.Data.MySqlClient.MySqlConnection(serverConnectionString))
        {
            await connection.OpenAsync();
            
            var createDbCommand = connection.CreateCommand();
            createDbCommand.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
            await createDbCommand.ExecuteNonQueryAsync();
            
            logger.LogInformation($"Database '{databaseName}' created or verified to exist.");
        }
        
        // Now try to ensure tables exist
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Database tables created successfully.");
        
        // Seed data
        await SeedTestDataAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Manual database creation failed: {ErrorMessage}", ex.Message);
        throw;
    }
}
