using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Contracts;

public interface IOrderRepository : IRepository<Order>
{
    Task AddOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken);
}