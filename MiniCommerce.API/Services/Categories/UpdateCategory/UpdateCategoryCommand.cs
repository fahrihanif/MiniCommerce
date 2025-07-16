using MediatR;

namespace MiniCommerce.API.Services.Categories.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name) : IRequest;