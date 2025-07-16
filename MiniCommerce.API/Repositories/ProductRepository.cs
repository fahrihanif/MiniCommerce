using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
{
    
}