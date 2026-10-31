using ProductosAPI.Models;
using ProductosAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Swagger activo siempre, para poder probar la API
app.UseSwagger();
app.UseSwaggerUI();

// GET /productos  -> todos
app.MapGet("/productos", (IProductoRepository repo) => Results.Ok(repo.ObtenerTodos()));

// GET /productos/buscar?texto=...  -> búsqueda por nombre
app.MapGet("/productos/buscar", (string texto, IProductoRepository repo) => Results.Ok(repo.Buscar(texto)));

// GET /productos/{id}  -> uno por id
app.MapGet("/productos/{id:int}", (int id, IProductoRepository repo) =>
{
    var p = repo.ObtenerPorId(id);
    return p == null ? Results.NotFound() : Results.Ok(p);
});

// POST /productos  -> crear
app.MapPost("/productos", (Producto nuevo, IProductoRepository repo) =>
{
    nuevo.Id = repo.Insertar(nuevo);
    return Results.Created($"/productos/{nuevo.Id}", nuevo);
});

// PUT /productos/{id}  -> actualizar
app.MapPut("/productos/{id:int}", (int id, Producto datos, IProductoRepository repo) =>
{
    datos.Id = id;
    return repo.Actualizar(datos) ? Results.Ok(datos) : Results.NotFound();
});

// DELETE /productos/{id}  -> eliminar
app.MapDelete("/productos/{id:int}", (int id, IProductoRepository repo) =>
    repo.Eliminar(id) ? Results.NoContent() : Results.NotFound());

app.Run();
