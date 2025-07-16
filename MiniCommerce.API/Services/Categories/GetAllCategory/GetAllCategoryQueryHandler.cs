using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Categories.GetAllCategory;

public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<GetCategoryResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetAllCategoryQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetCategoryResponse>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        
        return categories.Select(c => new GetCategoryResponse(c.Id, c.Name));
    }
}