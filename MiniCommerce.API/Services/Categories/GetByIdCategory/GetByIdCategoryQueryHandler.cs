using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Data;
using MiniCommerce.API.Services.Categories.GetAllCategory;

namespace MiniCommerce.API.Services.Categories.GetByIdCategory;

public class GetByIdCategoryQueryHandler : IQueryHandler<GetByIdCategoryQuery, GetCategoryResponse>
{
    private readonly ApplicationDbContext _context;

    public GetByIdCategoryQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetCategoryResponse>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Where(c => c.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (category is null)
            return Result.Failure<GetCategoryResponse>(CategoryErrors.Empty);

        var mapToCategoryResponse = new GetCategoryResponse(category.Id, category.Name);
        
        return Result.Success(mapToCategoryResponse);
    }
}