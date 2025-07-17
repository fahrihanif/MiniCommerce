using MediatR;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ApplicationDbContext context)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(request.Id, cancellationToken);
        
        if (category == null)
            return Result.Failure(CategoryErrors.InvalidId);
        
        await categoryRepository.RemoveAsync(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}