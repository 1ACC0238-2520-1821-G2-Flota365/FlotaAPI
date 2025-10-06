-- Script para configurar la base de datos MySQL para el backend
-- Ejecutar este script en MySQL Workbench o línea de comandos de MySQL

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS backend_f365_db 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

-- Usar la base de datos
USE backend_f365_db;

-- Verificar que el usuario root puede acceder
SELECT 'Base de datos creada correctamente' as status;

-- Mostrar las tablas existentes (si las hay)
SHOW TABLES;

-- Mostrar el estado de la base de datos
SELECT 
    SCHEMA_NAME as database_name,
    DEFAULT_CHARACTER_SET_NAME as charset,
    DEFAULT_COLLATION_NAME as collation
FROM information_schema.SCHEMATA 
WHERE SCHEMA_NAME = 'backend_f365_db';
