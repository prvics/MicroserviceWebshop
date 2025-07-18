using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Product 1", Price = 100 },
        new Product { Id = 2, Name = "Product 2", Price = 200 },
        new Product { Id = 3, Name = "Product 3", Price = 300 }
    };


    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product? GetProduct(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public void Add(Product product)
    {
        if (product.Id == 0) product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        if (_products.Any(p => p.Id == product.Id))
            throw new InvalidOperationException($"Product with id {product.Id} already exists.");
        _products.Add(product);
    }

    public void Update(Product product)
    {
        var existing = GetProduct(product.Id);
        if (existing == null) return;
        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Description = product.Description;
        existing.ImageUrl = product.ImageUrl;
    }

    public void Delete(int id)
    {
        var product = GetProduct(id);
        if (product == null) return;
        _products.Remove(product);
    }
}