using Microsoft.AspNetCore.Mvc;
using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService _productService) : ControllerBase
{
    
    [HttpGet]
    public ActionResult GetAll()
    {
        var products = _productService.GetAll();
        if (products == null || !products.Any()) return NotFound();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = _productService.GetProduct(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
    
    [HttpPost]
    public IActionResult Create(Product product)
    {
        try
        {
            _productService.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (InvalidOperationException e) when (e.Message.Contains("already exists"))
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Product product)
    {
        if (_productService.GetProduct(id) == null) return NotFound();
        if (id != product.Id) return BadRequest();
        product.Id = id;
        _productService.Update(product);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_productService.GetProduct(id) == null) return NotFound();
        _productService.Delete(id);
        return NoContent();
    }
}