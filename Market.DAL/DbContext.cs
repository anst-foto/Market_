using System.Data;
using Market.Models;
using Npgsql;

namespace Market.DAL;

public class DbContext : ICrud<Product>
{
    private const string ConnectionString = 
        "Server=127.0.0.1;Port=5432;Database=market_db;User Id=postgres;Password=1234;";
    private readonly NpgsqlConnection _connection;

    public DbContext()
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }
    
    public IEnumerable<Product> GetAll()
    {
        var products = new List<Product>();
        
        _connection.Open();
        
        const string sql = "SELECT id, name, price FROM table_products";
        var command = new NpgsqlCommand(sql, _connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("name");
                var price = reader.GetDecimal("price");
                
                products.Add(new Product(id, name, price));
            }
        }
        
        _connection.Close();
        
        return products;
    }

    public bool Insert(Product entity)
    {
        const string sql = """
                           INSERT INTO table_products (name, price) 
                           VALUES (@name, @price);
                           """;
        var parameters = new Dictionary<string, object>
        {
            {"name", entity.Name}, 
            {"price", entity.Price}
        };
        
        return Exec(sql, parameters);
    }

    public bool Update(Product entity)
    {
        const string sql = """
                           UPDATE table_products
                           SET name = @name, price = @price
                           WHERE id = @id;
                           """;
        
        return Exec(sql, entity.ToParameters());
    }

    public bool Delete(Product entity)
    {
       const string sql = "DELETE FROM table_products WHERE id = @id;";
       
       var parameters = new Dictionary<string, object>
       {
           {"id", entity.Id}
       };
       
       return Exec(sql, parameters);
    }
    
    private bool Exec(string sql, Dictionary<string, object> parameters)
    {
        _connection.Open();
        
        var command = new NpgsqlCommand(sql, _connection);

        foreach (var (key, value) in parameters)
        {
            command.Parameters.AddWithValue(key, value);
        }
        var result = command.ExecuteNonQuery();
        
        _connection.Close();
        
        return result > 0;
    }
}