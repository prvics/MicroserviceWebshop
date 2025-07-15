using ProductService.Interfaces;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public IEnumerable<Product> GetAll()
    {
        return productRepository.GetAll();
    }

    public Product? GetProduct(int id)
    {
        return productRepository.GetProduct(id);
    }

    public void Add(Product product)
    {
        productRepository.Add(product);
    }

    public void Update(Product product)
    {
        productRepository.Update(product);
    }

    public void Delete(int id)
    {
        productRepository.Delete(id);
    }
}