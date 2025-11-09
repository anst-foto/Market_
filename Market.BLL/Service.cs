using Market.DAL;
using Market.Models;

namespace Market.BLL;

public class Service
{
    private readonly ICrud<Product> _crud;

    public Service()
    {
        _crud = new DbContextWithDapper();
    }
    
    public IEnumerable<Product> GetAll() => _crud.GetAll();

    public IEnumerable<Product> GetByName(string name)
    {
        var products = GetAll();
        return products.Where(product => product.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
    }
    
    public bool Add(Product product) => _crud.Insert(product);
    public bool Delete(Product product) => _crud.Delete(product);
    public bool Update(Product product) => _crud.Update(product);
}