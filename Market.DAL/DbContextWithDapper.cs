using Dapper;
using Market.Models;
using Npgsql;

namespace Market.DAL;

public class DbContextWithDapper : ICrud<Product>
{
    private const string ConnectionString = 
        "Server=127.0.0.1;Port=5432;Database=market_db;User Id=postgres;Password=1234;";
    private readonly NpgsqlConnection _connection;

    public DbContextWithDapper()
    {
        _connection = new NpgsqlConnection(ConnectionString);
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public IEnumerable<Product> GetAll()
    {
        _connection.Open();
        
        const string sql = "SELECT id, name, price FROM table_products";
        var products = _connection.Query<Product>(sql);
        
        _connection.Close();
        
        return products;
    }

    public bool Insert(Product entity)
    {
        const string sql = """
                           INSERT INTO table_products (name, price) 
                           VALUES (@name, @price);
                           """;
        return Exec(sql, entity);
    }

    public bool Update(Product entity)
    {
        const string sql = """
                           UPDATE table_products
                           SET name = @name, price = @price
                           WHERE id = @id;
                           """;
        return Exec(sql, entity);
    }

    public bool Delete(Product entity)
    {
        const string sql = "DELETE FROM table_products WHERE id = @id;";
        return Exec(sql, entity);
    }
    
    private bool Exec(string sql, Product entity)
    {
        _connection.Open();
        var result = _connection.Execute(sql, entity);
        _connection.Close();
        
        return result > 0;
    }
}