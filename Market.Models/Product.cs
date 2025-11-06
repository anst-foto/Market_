namespace Market.Models;

/*public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}*/

public record Product(int Id, string Name, decimal Price);