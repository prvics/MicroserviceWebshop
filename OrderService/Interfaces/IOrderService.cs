using OrderService.Models;

namespace OrderService.Interfaces;

public interface IOrderService
{
    IEnumerable<Order> GetALl();
    Order? GetOrder(int id);
    void Add(Order order);
    void Update(Order order);
    void Delete(int id);
}