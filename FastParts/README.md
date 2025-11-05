# Instalar y configurar

Instalar Entity Framework
Abre Visual Studio y ve a Tools > NuGet Package Manager > Package Manager Console.

Comando de Restauración

PM>  Update-Package -Reinstall

PM> Install-Package EntityFramework

Habilitar migraciones

PM> Enable-Migrations -EnableAutomaticMigrations

Crear y actualizar la base de datos

PM> Add-Migration CrearBaseDeDatos -Verbose

PM> Update-Database -Verbose


# Restauración

# Borrar FastParts en (localdb)\MSSQLLocalDB

PM>  Update-Package -Reinstall
PM> Update-Database -Verbose