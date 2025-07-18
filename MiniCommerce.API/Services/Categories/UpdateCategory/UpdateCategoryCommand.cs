using MediatR;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Categories.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name) : ICommand;