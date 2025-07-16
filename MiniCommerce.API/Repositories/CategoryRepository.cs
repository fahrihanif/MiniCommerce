using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class CategoryRepository(ApplicationDbContext context) : Repository<Category>(context), ICategoryRepository
{
    
}