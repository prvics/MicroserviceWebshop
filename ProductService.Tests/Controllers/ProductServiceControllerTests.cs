using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Tests.Controllers;

public class ProductServiceControllerTests
{
    private Mock<IProductService> _mockService;
    private ProductController _productController;
    private List<Product> _products;
    
    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IProductService>();
        _products = new List<Product>
        {
            new() { Id = 1, Name = "Test1", Price = 100 },
            new() { Id = 2, Name = "Test2", Price = 200 },
            new() { Id = 3, Name = "Test3", Price = 300 }
        };
        _productController = new ProductController(_mockService.Object);
    }
    
    [Test]
    public void GetAll_ReturnsOk_WithProducts()
    {
        //arrange
        _mockService.Setup(s => s.GetAll()).Returns(_products);
        
        //act
        var result = _productController.GetAll();

        //assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.Value, Is.EqualTo(_products));
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
    }
    
    [Test]
    public void GetAll_ReturnsNotFound_WhenNoProducts()
    {
        //arrange
        _mockService.Setup(s => s.GetAll()).Returns(new List<Product>());
        
        //act
        var result = _productController.GetAll();
        
        //assert
        var notFoundResult = result as NotFoundResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }
    
    [Test]
    public void GetProduct_ReturnsOk_WithProduct()
    {
        //arrange
        _mockService.Setup(s => s.GetProduct(1)).Returns(_products[0]);
        
        //act
        var result = _productController.GetProduct(1);
        
        //assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.Value, Is.EqualTo(_products[0]));
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void GetProduct_ReturnsNotFound_WhenNoProduct()
    {
        //arrange
        _mockService.Setup(s => s.GetProduct(1)).Returns((Product?)null);
        
        //act
        var result = _productController.GetProduct(1);
        
        //assert
        var notFoundResult = result.Result as NotFoundResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public void Create_ReturnsCreated_WithProduct()
    {
        //arrange
        var addableProduct = new Product { Name = "Test", Price = 100 };
        _mockService.Setup(s => s.GetAll()).Returns(_products);
        _mockService.Setup(s => s.Add(addableProduct));
        
        //act
        _productController.Create(addableProduct);
        var result = _productController.GetAll();
        
        //assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.Value, Is.EqualTo(_products));
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
    }
    
    [Test]
    public void Create_InternalServerError_WhenAddFails()
    {
        //arrange
        var addableProduct = new Product { Name = "Test", Price = 100 };
        _mockService.Setup(s => s.GetAll()).Returns(_products);
        _mockService.Setup(s => s.Add(addableProduct)).Throws(new Exception());
        
        //act
        var result = _productController.Create(addableProduct);
        
        //arrange
        var internalServerErrorResult = result as StatusCodeResult;
        Assert.That(internalServerErrorResult, Is.Not.Null);
        Assert.That(internalServerErrorResult?.StatusCode, Is.EqualTo(500));
    }
    
    [Test]
    public void Create_Conflict_WhenAddFails()
    {
        //arrange
        var addableProduct = new Product { Name = "Test", Price = 100 };
        _mockService.Setup(s => s.GetAll()).Returns(_products);
        _mockService.Setup(s => s.Add(addableProduct)).Throws(new InvalidOperationException("Product already exists."));
        
        //act
        var result = _productController.Create(addableProduct);
        
        //assert
        var conflictResult = result as ConflictObjectResult;
        Assert.That(conflictResult, Is.Not.Null);
        Assert.That(conflictResult?.Value, Is.EqualTo("Product already exists."));
        Assert.That(conflictResult?.StatusCode, Is.EqualTo(409));
    }

    [Test]
    public void Update_ReturnsNoContent_WhenUpdateSucceeds()
    {
        //arrange
        var updateableProduct = new Product { Id = 1, Name = "Test", Price = 100 };
        _mockService.Setup(s => s.GetProduct(1)).Returns(updateableProduct);
        _mockService.Setup(s => s.Update(updateableProduct));
        
        //act
        var result = _productController.Update(1, updateableProduct);
        
        //assert
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult, Is.Not.Null);
        Assert.That(noContentResult?.StatusCode, Is.EqualTo(204));
    }
    
    [Test]
    public void Update_BadRequestResultIdDoNotMatch_WhenUpdateFails()
    {
        //arrange
        var updateableProduct = new Product { Id = 58, Name = "Test", Price = 100 };
        _mockService.Setup(s => s.GetProduct(59)).Returns(updateableProduct);
        
        //act
        var result = _productController.Update(59, updateableProduct);
        
        //assert
        var badRequestResult = result as BadRequestResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult?.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public void Update_NotFoundResult_WhenUpdateFails()
    {
        //arrange no need to SetUp ;( there is no product with ID 77
        
        //act
        var result = _productController.Update(77, new Product { Id = 1, Name = "Test", Price = 100 });
        
        //assert
        var notFoundResult = result as NotFoundResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }
    
    [Test]
    public void Delete_ReturnsNoContent_WhenDeleteSucceeds()
    {
        //arrange
        var product = new Product { Id = 69, Name = "ToBeDeleted", Price = 69 };
        _mockService.Setup(s => s.GetProduct(69)).Returns(product);

        //act
        var result = _productController.Delete(69);

        //assert
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult, Is.Not.Null);
        Assert.That(noContentResult?.StatusCode, Is.EqualTo(204));
    }
    
    [Test]
    public void Delete_NotFoundResult_WhenDeleteFails()
    {
        //arrange no need to SetUp ;( there is no product with ID 99)
        
        //act
        var result = _productController.Delete(99);
        
        //assert
        var notFoundResult = result as NotFoundResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404));
    }
    
}