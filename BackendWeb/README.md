# Fleet Management Backend API

API backend para gestión de flotas vehiculares con MySQL y Entity Framework.

## 🚀 Características

- ✅ **MySQL con Entity Framework Core** (usando Pomelo.EntityFrameworkCore.MySql)
- ✅ **Migraciones automáticas** al iniciar la aplicación
- ✅ **Swagger UI** para documentación y pruebas de API
- ✅ **Arquitectura por dominio** (DDD)
- ✅ **Gestión de**: Vehículos, Conductores, Asignaciones, Mantenimiento, Reportes

## 📋 Prerrequisitos

Para ejecutar este proyecto en cualquier computadora necesitas:

1. **.NET 8.0 SDK** o superior
2. **MySQL Server** (versión 8.0 o superior recomendado)
3. Conexión a internet (para descargar paquetes NuGet)

## ⚙️ Configuración

### 1. Configurar la base de datos MySQL

En `appsettings.json` configura tu connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=backend_f365_db;user=root;password=TU_PASSWORD;"
  }
}
```

**Importante**: Cambia `TU_PASSWORD` por tu contraseña real de MySQL.

### 2. Crear la base de datos (opcional)

La aplicación creará automáticamente la base de datos y las tablas, pero si prefieres crearla manualmente:

```sql
CREATE DATABASE backend_f365_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

## 🚀 Ejecución

### Método 1: Ejecución directa

```bash
# Clonar/descargar el proyecto
cd BackendWeb

# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run
```

### Método 2: Con compilación explícita

```bash
# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run
```

## 📊 Acceso a la aplicación

Una vez ejecutada, la aplicación estará disponible en:

- **HTTP**: http://localhost:5047
- **Swagger UI**: http://localhost:5047/swagger

## 🗄️ Migraciones automáticas

La aplicación aplica automáticamente las migraciones al iniciar, esto significa que:

✅ **Crea la base de datos** si no existe  
✅ **Crea todas las tablas** necesarias  
✅ **Aplica cambios de esquema** automáticamente  
✅ **Funciona en cualquier computadora** con MySQL instalado  

### Tablas creadas automáticamente:

- `Assignments` - Asignaciones de vehículos a conductores
- `Drivers` - Información de conductores  
- `MaintenanceOrders` - Órdenes de mantenimiento
- `Managers` - Gestores del sistema
- `Reports` - Reportes generados
- `Vehicles` - Información de vehículos

## 🛠️ Comandos útiles de Entity Framework

Si necesitas trabajar con migraciones manualmente:

```bash
# Ver migraciones pendientes
dotnet ef migrations list

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones manualmente (no necesario con auto-migration)
dotnet ef database update

# Revertir migración
dotnet ef database update MigracionAnterior
```

## 🔧 Solución de problemas

### Error de conexión MySQL

Si obtienes errores de conexión:

1. Verifica que MySQL esté ejecutándose
2. Confirma el usuario y contraseña en `appsettings.json`
3. Verifica que el puerto (3306) esté correcto
4. Asegúrate de que el usuario tiene permisos para crear bases de datos

### Error de migraciones

Si hay errores con las migraciones automáticas:

```bash
# Borrar migraciones y recrear
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
```

## 📁 Estructura del proyecto

```
BackendWeb/
├── Assignments/          # Gestión de asignaciones
├── DriverManagement/     # Gestión de conductores  
├── FleetManagement/      # Gestión de vehículos
├── Maintenance/          # Gestión de mantenimiento
├── Management/           # Gestión administrativa
├── Reporting/            # Gestión de reportes
├── Shared/
│   └── Persistance/EFC/  # Configuración Entity Framework
├── Extensions/           # Métodos de extensión
└── Properties/           # Configuración de la aplicación
```

## 🎯 Endpoints principales

La API incluye endpoints para:

- **GET/POST** `/api/vehicle` - Gestión de vehículos
- **GET/POST** `/api/driver` - Gestión de conductores  
- **GET/POST** `/api/assignment` - Gestión de asignaciones
- **GET/POST** `/api/maintenance` - Gestión de mantenimiento

Consulta la documentación completa en Swagger UI cuando la aplicación esté ejecutándose.
