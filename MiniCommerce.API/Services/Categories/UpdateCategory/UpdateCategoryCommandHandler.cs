using MediatR;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ApplicationDbContext context,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(request.Id, cancellationToken);
        
        if (category == null)
            return;
        
        category.Name = request.Name;
        
        await categoryRepository.UpdateAsync(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}