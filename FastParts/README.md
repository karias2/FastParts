# Instalar y configurar

Instalar Entity Framework
Abre Visual Studio y ve a Tools > NuGet Package Manager > Package Manager Console.

> Install-Package EntityFramework

Habilitar migraciones

> Enable-Migrations -EnableAutomaticMigrations

Crear y actualizar la base de datos

> Add-Migration NOMBRE_DEL_CAMBIO -Verbose

> Update-Database -Verbose