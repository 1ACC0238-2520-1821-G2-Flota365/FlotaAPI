# Backend Fleet Management - Instrucciones de Configuración

## 🚀 Pasos para ejecutar el backend

### 1. Verificar MySQL

Antes de ejecutar el backend, asegúrate de que MySQL esté funcionando:

```bash
# Verificar si MySQL está ejecutándose
# En Windows, puedes usar:
services.msc
# O desde PowerShell:
Get-Service -Name "MySQL*"
```

### 2. Configurar la Base de Datos

Ejecuta el script SQL incluido para crear la base de datos:

```sql
-- En MySQL Workbench o línea de comandos de MySQL:
mysql -u root -p11223344 < setup-database.sql
```

O manualmente:
```sql
CREATE DATABASE IF NOT EXISTS backend_f365_db 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;
```

### 3. Configuración de la aplicación

El archivo `appsettings.json` ya tiene la configuración correcta:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=backend_f365_db;user=root;password=11223344;"
  }
}
```

### 4. Ejecutar el Backend

```bash
# Desde el directorio del proyecto:
cd "C:\Users\user\Desktop\final app\BackendWeb\BackendWeb"

# Restaurar paquetes NuGet
dotnet restore

# Ejecutar la aplicación
dotnet run
```

### 5. Verificar que funciona

Una vez que la aplicación esté ejecutándose, puedes verificarla:

- **Swagger UI**: http://localhost:5047/swagger/index.html
- **Health Check**: http://localhost:5047/api/health
- **Health Info**: http://localhost:5047/api/health/info

### 6. Endpoints disponibles

El backend expone los siguientes endpoints principales:

#### Dashboard
- GET `/api/dashboard/stats` - Estadísticas del dashboard
- GET `/api/dashboard/active-vehicles` - Vehículos activos  
- GET `/api/dashboard/fleet-summary` - Resumen de flota

#### Drivers  
- GET `/api/driver` - Obtener todos los conductores
- GET `/api/driver/{id}` - Obtener conductor por ID
- GET `/api/driver/stats` - Estadísticas de conductores
- POST `/api/driver` - Crear conductor
- PUT `/api/driver/{id}` - Actualizar conductor
- DELETE `/api/driver/{id}` - Eliminar conductor

#### Vehicles
- GET `/api/vehicle` - Obtener todos los vehículos
- GET `/api/vehicle/{id}` - Obtener vehículo por ID
- POST `/api/vehicle` - Crear vehículo
- PUT `/api/vehicle/{id}` - Actualizar vehículo  
- DELETE `/api/vehicle/{id}` - Eliminar vehículo

#### Y muchos más... (ver Swagger para lista completa)

## 🔧 Solución de Problemas

### Error de conexión a la base de datos

1. Verifica que MySQL esté ejecutándose
2. Verifica que la base de datos `backend_f365_db` exista
3. Verifica las credenciales en `appsettings.json`

### Error 500 en endpoints

1. Revisa los logs en la consola del backend
2. Verifica el endpoint de salud: `http://localhost:5047/api/health`
3. Si la base de datos está vacía, el backend automáticamente creará datos de prueba

### CORS errors desde el frontend

Ya está configurado para permitir requests desde:
- http://localhost:8082 (frontend Vue)
- http://localhost:8080
- http://localhost:3000

## 📝 Datos de Prueba

El backend automáticamente creará datos de prueba si la base de datos está vacía:
- 3 vehículos de muestra
- 2 conductores de muestra  
- Tablas de mantenimiento (vacías inicialmente)

## 🐛 Debug

Para más información de debug, revisa:
- Los logs en la consola donde ejecutas `dotnet run`
- El endpoint `/api/health/info` para detalles del sistema
- Swagger UI para probar endpoints individualmente

## 🔐 Autenticación

El sistema incluye autenticación JWT, pero para desarrollo puedes probar los endpoints sin autenticación primero.
