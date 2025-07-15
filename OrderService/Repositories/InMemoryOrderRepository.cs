using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new()
    {
        new Order { Id = 1, ProductId = 1, Quantity = 1 },
        new Order { Id = 2, ProductId = 2, Quantity = 2 },
        new Order { Id = 3, ProductId = 3, Quantity = 3 }
    };
    
    public IEnumerable<Order> GetALl()
    {
        return _orders;
    }

    public Order? GetOrder(int id)
    {
        return _orders.FirstOrDefault(o => o.Id == id);
    }
    
    public void Add(Order order)
    {
        order.Id = _orders.Count > 0 ? _orders.Max(o => o.Id) + 1 : 1;
        _orders.Add(order);
    }

    public void Update(Order order)
    {
        var existing = GetOrder(order.Id);
        if (existing == null) return;
        existing.ProductId = order.ProductId;
        existing.Quantity = order.Quantity;
    }

    public void Delete(int id)
    {
        var order = GetOrder(id);
        if (order == null) return;
        _orders.Remove(order);
    }
}