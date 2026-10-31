# ProductosAPI CRUD de productos (C# + SQL Server Managment studio (Es el que tenia instalado profe)+ Swagger)

## Flujo
1. En SSMS se ejecuta `script.sql`: se crea la base `ProductosDB` y la tabla `Productos`.
2. `appsettings.json` guarda la cadena de conexión a SQL Server.
3. `Program.cs` registra el repositorio y expone los endpoints REST.
4. El cliente (Swagger) llama a un endpoint, por ejemplo `POST /productos`.
5. El endpoint llama al método del repositorio (`IProductoRepository`).
6. `ProductoRepository` abre la conexión, ejecuta la consulta SQL con parámetros y regresa el resultado.
7. El endpoint responde en JSON con el código HTTP correspondiente (200, 201, 204 o 404).