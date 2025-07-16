using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Contracts;

public interface ICartRepository : IRepository<Cart>
{
    Task AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);
    Task RemoveCartItemAsync(CartItem cartItem);
}