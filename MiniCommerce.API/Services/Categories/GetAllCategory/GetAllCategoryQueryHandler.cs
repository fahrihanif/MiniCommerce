using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Categories.GetAllCategory;

public class GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, IEnumerable<GetCategoryResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetAllCategoryQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<GetCategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        
        if (!categories.Any())
            Result.Failure<IEnumerable<GetCategoryResponse>>(CategoryErrors.InvalidId);
        
        var mapToCategoryResponse = categories.Select(c => new GetCategoryResponse(c.Id, c.Name));
        
        return Result.Success(mapToCategoryResponse);
    }
}