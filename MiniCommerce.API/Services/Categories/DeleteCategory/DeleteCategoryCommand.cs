using MediatR;

namespace MiniCommerce.API.Services.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest;