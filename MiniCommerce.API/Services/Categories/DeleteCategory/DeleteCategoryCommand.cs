using MediatR;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand;