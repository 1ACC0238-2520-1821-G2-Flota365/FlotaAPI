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

// -------------------- JWT CONFIG --------------------
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

// -------------------- DATABASE (MySQL) --------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10)
        )
    )
);

// -------------------- DEPENDENCY INJECTION --------------------

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
builder.Services.AddScoped<IMaintenanceRecordRepository>(provider =>
    provider.GetService<IMaintenanceRepository>() as IMaintenanceRecordRepository);
builder.Services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
builder.Services.AddScoped<MaintenanceService>();

// Reporting
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();

// Management
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<ManagerService>();

// Auth
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

// Dashboard
builder.Services.AddScoped<DashboardService>();

// Fleets
builder.Services.AddScoped<IFleetRepository, FleetRepository>();
builder.Services.AddScoped<FleetService>();

// -------------------- CORS --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8082",
                "http://localhost:8080",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// -------------------- CONTROLLERS + SWAGGER --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------- PORT --------------------
// Forzar que la app escuche en el puerto asignado por Render/Docker/Koyeb
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// -------------------- MIGRATIONS & SEED DATA --------------------
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Applying database migrations...");

    dbContext.Database.Migrate();

    logger.LogInformation("Database migrations applied successfully.");
    await SeedTestDataAsync(dbContext, logger);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Error during database setup: {Message}", ex.Message);
}

// -------------------- SWAGGER UI --------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fleet Management API V1");
    c.RoutePrefix = "swagger"; // Accesible en /swagger
});

// -------------------- EXCEPTION HANDLING --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

// -------------------- MIDDLEWARE --------------------
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// -------------------- CONTROLLERS --------------------
app.MapControllers();

app.Run();

// =================== HELPER METHODS ===================
static async Task SeedTestDataAsync(AppDbContext context, ILogger logger)
{
    try
    {
        logger.LogInformation("Checking if test data seeding is needed...");

        var vehicleCount = await context.Vehicles.CountAsync();
        var driverCount = await context.Drivers.CountAsync();

        if (vehicleCount == 0 && driverCount == 0)
        {
            logger.LogInformation("Database is empty. Seeding with test data...");

            // Vehicles
            context.Vehicles.AddRange(
                new BackendWeb.FleetManagement.Domain.Vehicle("ABC123", "Toyota", "Hiace", 2020, 15000, 1, "Flota Principal"),
                new BackendWeb.FleetManagement.Domain.Vehicle("XYZ789", "Ford", "Transit", 2019, 25000, 1, "Flota Principal"),
                new BackendWeb.FleetManagement.Domain.Vehicle("DEF456", "Chevrolet", "Spark", 2021, 8000, 2, "Flota Secundaria")
            );

            // Drivers
            context.Drivers.AddRange(
                new BackendWeb.DriverManagement.Domain.Driver("DRV001", "Juan", "Pérez", "L12345678", DateTime.UtcNow.AddYears(2), "+51987654321", "juan@email.com", 5),
                new BackendWeb.DriverManagement.Domain.Driver("DRV002", "María", "García", "L87654321", DateTime.UtcNow.AddYears(3), "+51123456789", "maria@email.com", 8)
            );

            // Admin user
            var sampleUser = new BackendWeb.Auth.Domain.User(
                "admin@flota365.com",
                BCrypt.Net.BCrypt.HashPassword("admin123"),
                "Admin",
                "System",
                "Admin"
            );

            context.Users.Add(sampleUser);

            await context.SaveChangesAsync();
            logger.LogInformation("✅ Test data seeded successfully!");
        }
        else
        {
            logger.LogInformation("Database already contains data. Skipping seeding.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occurred while seeding test data: {ErrorMessage}", ex.Message);
    }
}
