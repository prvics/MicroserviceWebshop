using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Tests.Repositoris;

public class ProductServiceInMemoryRepositoryTests
{
    private InMemoryProductRepository _repo;
    
    [SetUp]
    public void Setup()
    {
        _repo = new InMemoryProductRepository();
    }
    
    [Test]
    public void GetAll_Returns_Products()
    {
        //arrange ;(
        
        //act
        var products = _repo.GetAll();
        
        //assert
        Assert.That(products, Is.Not.Null, "Products collection should not be null.");
        Assert.That(products, Is.Not.Empty, "Products collection should not be empty.");;
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
    }
    
    [Test]
    public void GetProduct_WhenProductExists()
    {
        //arrange ;(
        
        //act
        var product = _repo.GetProduct(1);
        
        //assert
        Assert.That(product, Is.Not.Null, "Product should not be null.");
        Assert.That(product.Id, Is.EqualTo(1), "Product ID should be 1.");
    }

    [Test]
    public void GetProduct_WhenProductDoesNotExists()
    {
        //arrange ;(
        
        //act
        var product = _repo.GetProduct(96);
        
        //assert
        Assert.That(product, Is.Null, "Should return null for missing id.");
    }

    [Test]
    public void Add_WithOutId_Success()
    {
        //arrange
        var product = new Product { Name = "Test", Price = 100 };
        
        //act
        _repo.Add(product);
        var products = _repo.GetAll();
        
        //assert
        Assert.That(products.Count(), Is.EqualTo(4), "Products collection should have 4 items.");
        Assert.That(products.Any(p => p.Name == "Test"), Is.True, "Product should be added to repository.");;
    }
    
    [Test]
    public void Add_WithId_Success()
    {
        //arrange
        var product = new Product { Id = 69, Name = "Test", Price = 100 };
        
        //act
        _repo.Add(product);
        var products = _repo.GetAll();
        
        //assert
        Assert.That(products.Count(), Is.EqualTo(4), "Products collection should have 4 items.");
        Assert.That(products.Any(p => p.Name == "Test"), Is.True, "Product should be added to repository.");
        Assert.That(products.Any(p => p.Id == 69), Is.True, "Product should be added to repository.");
    }
    
    [Test]
    public void Add_AlreadyExists_Failure()
    {
        //arrange
        var product = new Product { Id = 1, Name = "Test", Price = 100 };
        
        //act + assert
        Assert.Throws<InvalidOperationException>(() => _repo.Add(product));
        
        //assert
        var products = _repo.GetAll();
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
        Assert.That(products.Any(p => p.Name == "Test"), Is.False, "Product should not be added to repository.");
    }
    
    [Test]
    public void Update_WhenProductExists()
    {
        //arrange
        var product = new Product { Id = 1, Name = "Test", Price = 100 };
        
        //act
        _repo.Update(product);
        
        //assert
        var products = _repo.GetAll();
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
        Assert.That(products.Any(p => p.Name == "Test"), Is.True, "Product should be added to repository.");
        Assert.That(products.Any(p => p.Id == 1), Is.True, "Product should be added to repository.");
        Assert.That(products.Any(p => p.Name == "Test" && p.Price == 100), Is.True, "Product should be added to repository.");
    }

    [Test]
    public void Update_WhenProductDoesNotExists()
    {
        //arrange
        var product = new Product { Id = 96, Name = "Test", Price = 100 };
        
        //act + assert
        Assert.Throws<InvalidOperationException>(() => _repo.Update(product));
        
        //assert
        var products = _repo.GetAll();
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
    }
    
    [Test]
    public void Delete_WhenProductExists()
    {
        //arrange
        var product = new Product { Id = 69, Name = "DeleteTest", Price = 69 };
        _repo.Add(product);
        
        //act
        var beforeDeleteCount = _repo.GetAll().Count();
        _repo.Delete(69);
        
        //assert
        var products = _repo.GetAll();
        Assert.That(beforeDeleteCount, Is.EqualTo(4), "Products collection should have 4 items.");
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
        Assert.That(products.Any(p => p.Name == "DeleteTest"), Is.False, "Product should be added to repository.");
    }
    
    [Test]
    public void Delete_WhenProductDoesNotExists()
    {
        //arrange no need to ;(
        
        //act + assert
        Assert.Throws<InvalidOperationException>(() => _repo.Delete(96));
        
        //assert
        var products = _repo.GetAll();
        Assert.That(products.Count(), Is.EqualTo(3), "Products collection should have 3 items.");
    }
}