namespace MiniCommerce.API.Common;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
