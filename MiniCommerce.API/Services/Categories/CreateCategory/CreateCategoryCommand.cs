using MediatR;

namespace MiniCommerce.API.Services.Categories.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<int>;