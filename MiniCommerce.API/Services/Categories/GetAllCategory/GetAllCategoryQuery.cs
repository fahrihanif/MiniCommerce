using MediatR;

namespace MiniCommerce.API.Services.Categories.GetAllCategory;

public record GetAllCategoryQuery() : IRequest<IEnumerable<GetCategoryResponse>>;