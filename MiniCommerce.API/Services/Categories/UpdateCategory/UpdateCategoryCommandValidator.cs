using FluentValidation;

namespace MiniCommerce.API.Services.Categories.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
            
        RuleFor(c => c.Name).NotEmpty();
    }
}