using MiniCommerce.API.Common;

namespace MiniCommerce.API.Services.Categories.GetAllCategory;

public static class CategoryErrors
{
    public static readonly Error IdNotFound = new(
        "Category.IdNotFound", 
        "The category with the specified ID was not found.");
}
