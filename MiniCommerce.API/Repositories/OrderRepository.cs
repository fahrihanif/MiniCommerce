using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class OrderRepository(ApplicationDbContext context) : Repository<Order>(context), IOrderRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken)
    {
        await _context.Set<OrderItem>().AddAsync(orderItem, cancellationToken);
    }
}