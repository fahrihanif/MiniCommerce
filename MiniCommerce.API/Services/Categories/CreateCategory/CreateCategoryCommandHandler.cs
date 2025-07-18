using MediatR;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Services.Categories.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        await categoryRepository.CreateAsync(category, cancellationToken);
        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(result);
    }
}