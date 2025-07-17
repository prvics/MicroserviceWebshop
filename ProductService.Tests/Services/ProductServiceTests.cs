using NUnit.Framework;
using System.Linq;
using ProductService.Models;
using ProductService.Interfaces;
using ProductService.Repositories;

namespace ProductService.Tests.Services;

public class ProductServiceTests
{
    private IProductRepository _productRepo;
    private IProductService _productService;
    
    [SetUp]
    public void Setup()
    {
        _productRepo = new InMemoryProductRepository();
        _productService = new ProductService.Services.ProductService(_productRepo);
    }
    
    [Test]
    public void GetAll_Returns_Products()
    {
        //arrange is handled in SetUp ;(
        
        //act
        var products = _productService.GetAll();

        //assert
        Assert.That(products, Is.Not.Null, "Products collection should not be null.");
        Assert.That(products, Is.Not.Empty, "Products collection should not be empty.");;
    }

    [Test]
    public void GetProduct_WhenProductExists()
    {
        //arrange is handled in SetUp ;(
        
        //act
        var product = _productService.GetProduct(1);
        
        //assert
        Assert.That(product, Is.Not.Null, "Product should not be null.");
        Assert.That(product.Id, Is.EqualTo(1), "Product ID should be 1.");
    }

    [Test]
    public void GetProduct_WhenProductDoesNotExists()
    {
        //arrange is handled in SetUp ;(
        
        //act
        var product = _productService.GetProduct(96);
        
        //assert
        Assert.That(product, Is.Null, "Should return null for missing id.");
    }

    [Test]
    public void Add_AddsProductToRepository()
    {
        //arrange
        var product = new Product
        {
            Name = "AddTest",
            Price = 3.0M
        };
        
        var beforeAddCount = _productService.GetAll().Count();
        
        //act
        _productService.Add(product);
        var products = _productService.GetAll();
        var addedProductName = products.Any(p => p.Name == "AddTest"); 
        var afterAddCount = products.Count();
        
        //assert
        Assert.That(afterAddCount, Is.EqualTo(beforeAddCount + 1), $"Product count should increase by one.");
        Assert.That(addedProductName, Is.True, "Product should be added to repository.");
    }

    [Test]
    public void Update_ChangesProductDetails()
    {
        //arrange
        var product = new Product
        {
            Id = 69,
            Name = "UpdateTest",
            Price = 4.0M
        };
        _productService.Add(product);
        var originalName = product.Name;
        product.Name = "UpdatedName";

        //act
        _productService.Update(product);
        var updatedProduct = _productService.GetProduct(69);
        
        //assert
        Assert.That(updatedProduct, Is.Not.Null, "Product should not be null.");
        Assert.That(updatedProduct.Name, Is.EqualTo("UpdatedName"), "Product name should be updated.");
        Assert.That(updatedProduct.Name, Is.Not.EqualTo(originalName), "Product name should not be the same as before.");
    }

    [Test]
    public void Delete_RemovesProduct()
    {
        //arrange
        var product = new Product
        {
            Id = 999,
            Name = "DeleteTest",
            Price = 1.0M
        };
        _productService.Add(product);
        var beforeDeleteCount = _productService.GetAll().Count();
        
        //act
        _productService.Delete(999);
        var products = _productService.GetAll();
        var afterDeleteCount = products.Count();
        var deletedProduct = products.Any(p => p.Id == 999);
        
        //assert
        Assert.That(afterDeleteCount, Is.EqualTo(beforeDeleteCount-1), "Product count should decrease by one.");
        Assert.That(deletedProduct, Is.False, "Product should be deleted from repository.");
    }
}