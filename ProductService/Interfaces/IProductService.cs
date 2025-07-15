using ProductService.Models;

namespace ProductService.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetProduct(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
}
