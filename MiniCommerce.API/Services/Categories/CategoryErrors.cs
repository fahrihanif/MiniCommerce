using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Categories;

public static class CategoryErrors
{
    public static readonly Error InvalidId = new Error(
        "Category.InvalidId", 
        "Category id is not found.");
    
    public static readonly Error Empty = new Error(
        "Category.Empty", 
        "Category is empty.");
}