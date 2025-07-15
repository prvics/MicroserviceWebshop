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
        return Ok(_productService.GetAll());
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
        _productService.Add(product);
        return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);
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