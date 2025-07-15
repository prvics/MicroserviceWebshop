using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Services;

public class OrderService(IOrderRepository _orderRepository) : IOrderService
{
    public IEnumerable<Order> GetALl()
    {
        return _orderRepository.GetALl();
    }

    public Order? GetOrder(int id)
    {
        return _orderRepository.GetOrder(id);
    }

    public void Add(Order order)
    {
        _orderRepository.Add(order);
    }

    public void Update(Order order)
    {
        _orderRepository.Update(order);
    }

    public void Delete(int id)
    {
        _orderRepository.Delete(id);
    }
}