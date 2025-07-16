using MediatR;
using MiniCommerce.API.Services.Categories.GetAllCategory;

namespace MiniCommerce.API.Services.Categories.GetByIdCategory;

public record GetByIdCategoryQuery(Guid Id) : IRequest<GetCategoryResponse?>;
