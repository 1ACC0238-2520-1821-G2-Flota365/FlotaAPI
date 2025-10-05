using Microsoft.EntityFrameworkCore;
using Shared.Persistence.EFC;

namespace BackendWeb.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Aplica automáticamente las migraciones pendientes a la base de datos.
        /// Crea la base de datos si no existe.
        /// </summary>
        /// <param name="app">La aplicación web</param>
        /// <returns>La aplicación web para encadenamiento</returns>
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            try
            {
                // Crear la base de datos si no existe y aplicar migraciones
                context.Database.Migrate();
                
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
                logger.LogInformation("Base de datos migrada exitosamente");
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
                logger.LogError(ex, "Error al migrar la base de datos");
                throw;
            }
            
            return app;
        }
    }
}
