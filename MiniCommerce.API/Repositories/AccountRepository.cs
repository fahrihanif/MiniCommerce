using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Repositories;

public class AccountRepository(ApplicationDbContext context) : 
    Repository<Account>(context), IAccountRepository
{
    
}