# Instalar y configurar

Instalar Entity Framework
Abre Visual Studio y ve a Tools > NuGet Package Manager > Package Manager Console.

PM> Install-Package EntityFramework

Habilitar migraciones

PM> Enable-Migrations -EnableAutomaticMigrations

Crear y actualizar la base de datos

PM> Add-Migration CrearBaseDeDatos -Verbose

PM> Update-Database -Verbose