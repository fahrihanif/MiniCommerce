using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Services.Categories.GetAllCategory;

namespace MiniCommerce.API.Services.Categories.GetByIdCategory;

public record GetByIdCategoryQuery(Guid Id) : IQuery<GetCategoryResponse>;
