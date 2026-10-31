using Microsoft.Data.SqlClient;
using ProductosAPI.Models;

namespace ProductosAPI.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly string _cadena;

    // CONEXIÓN: la cadena de conexión se lee de appsettings.json
    public ProductoRepository(IConfiguration config)
    {
        _cadena = config.GetConnectionString("Default")!;
    }

    // Convierte una fila de la base de datos en un objeto Producto
    private static Producto Leer(SqlDataReader r) => new Producto
    {
        Id = r.GetInt32(0),
        Nombre = r.GetString(1),
        Precio = r.GetDecimal(2),
        Stock = r.GetInt32(3)
    };

    // CONSULTA: todos los productos
    public List<Producto> ObtenerTodos()
    {
        var lista = new List<Producto>();
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand("SELECT Id, Nombre, Precio, Stock FROM Productos", con);
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read()) lista.Add(Leer(r));
        return lista;
    }

    // CONSULTA: un producto por id (null si no existe)
    public Producto? ObtenerPorId(int id)
    {
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand("SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @id", con);
        cmd.Parameters.AddWithValue("@id", id);
        con.Open();
        using var r = cmd.ExecuteReader();
        return r.Read() ? Leer(r) : null;
    }

    // INSERT: devuelve el Id generado
    public int Insertar(Producto p)
    {
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "INSERT INTO Productos (Nombre, Precio, Stock) OUTPUT INSERTED.Id VALUES (@nombre, @precio, @stock)", con);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        con.Open();
        return (int)cmd.ExecuteScalar()!;
    }

    // UPDATE: true si se modificó una fila
    public bool Actualizar(Producto p)
    {
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "UPDATE Productos SET Nombre = @nombre, Precio = @precio, Stock = @stock WHERE Id = @id", con);
        cmd.Parameters.AddWithValue("@id", p.Id);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        con.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // DELETE: true si se eliminó una fila
    public bool Eliminar(int id)
    {
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand("DELETE FROM Productos WHERE Id = @id", con);
        cmd.Parameters.AddWithValue("@id", id);
        con.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // BUSCAR: productos cuyo nombre contiene el texto
    public List<Producto> Buscar(string texto)
    {
        var lista = new List<Producto>();
        using var con = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(
            "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Nombre LIKE @texto", con);
        cmd.Parameters.AddWithValue("@texto", "%" + texto + "%");
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read()) lista.Add(Leer(r));
        return lista;
    }
}
