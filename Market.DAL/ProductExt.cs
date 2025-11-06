using Market.Models;

namespace Market.DAL;

public static class ProductExt
{
    public static Dictionary<string, object> ToParameters(this Product product)
    {
        var result = new Dictionary<string, object>();
        
        var type = product.GetType();
        var properties = type.GetProperties();
        foreach (var propertyInfo in properties)
        {
            result.Add(propertyInfo.Name, propertyInfo.GetValue(product));
        }
        
        return result;
    }
}