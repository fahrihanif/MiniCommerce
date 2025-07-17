using MiniCommerce.API.Common;

namespace MiniCommerce.API.Services.Categories.CreateCategory;

public record CreateCategoryCommand(string Name) : ICommand<int>;