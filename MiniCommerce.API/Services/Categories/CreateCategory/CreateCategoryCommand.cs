using MediatR;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Categories.CreateCategory;

public record CreateCategoryCommand(string Name) : ICommand;