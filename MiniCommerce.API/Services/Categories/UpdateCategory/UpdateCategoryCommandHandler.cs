using MediatR;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ApplicationDbContext context,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(request.Id, cancellationToken);
        
        if (category == null)
            return Result.Failure(CategoryErrors.InvalidId);
        
        category.Name = request.Name;
        
        await categoryRepository.UpdateAsync(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}