using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class CartRepository(ApplicationDbContext context) : Repository<Cart>(context), ICartRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
    {
        await _context.Set<CartItem>().AddAsync(cartItem, cancellationToken);
    }

    public Task RemoveCartItemAsync(CartItem cartItem)
    {
        _context.Set<CartItem>().Remove(cartItem);
        return Task.CompletedTask;
    }
}