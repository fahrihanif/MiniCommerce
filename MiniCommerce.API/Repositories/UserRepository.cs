using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class UserRepository(ApplicationDbContext context) : Repository<User>(context), IUserRepository
{
    
}