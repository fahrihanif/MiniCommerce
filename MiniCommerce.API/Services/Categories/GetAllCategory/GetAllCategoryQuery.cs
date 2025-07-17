using MediatR;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Categories.GetAllCategory;

public record GetAllCategoryQuery() : IQuery<IEnumerable<GetCategoryResponse>>;