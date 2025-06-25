# How to run project:

## Prerequisites
.NET 9, SQL Server, Visual Studio

## Steps
1. Pull repository on your local machine
2. Open solution in Visual Studio
3. Locate Solution in Solution Explorer
4. Right click on solution on select Restore NuGet Packages
5. Run build
6. Open Package Manager Console
7. Set Default project to Abysalto.Mid.Infrastructure
8. Create migration with following command :
```
Add-Migration {enter-migration-name}
```
9. Update database running command :
```
Update-Database
```
10. Run project in Visual Studio
