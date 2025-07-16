using MediatR;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Services.Categories.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommand, int>
{
    public async Task<int> Handle(CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        await categoryRepository.CreateAsync(category, cancellationToken);
        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return result;
    }
}