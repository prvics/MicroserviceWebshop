using Microsoft.AspNetCore.Mvc;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService _orderService) : ControllerBase
{
    [HttpGet]
    public ActionResult GetAll()
    {
        return Ok(_orderService.GetALl());
    }
    
    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        var order = _orderService.GetOrder(id);
        if (order == null) return NotFound();
        return Ok(order);
    }
    
    [HttpPost]
    public IActionResult Create(Order order)
    {
        _orderService.Add(order);
        return CreatedAtAction(nameof(GetOrder), new {id = order.Id}, order);
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Order order)
    {
        if (_orderService.GetOrder(id) == null) return NotFound();
        if (id != order.Id) return BadRequest();
        order.Id = id;
        _orderService.Update(order);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_orderService.GetOrder(id) == null) return NotFound();
        _orderService.Delete(id);
        return NoContent();
    }
}