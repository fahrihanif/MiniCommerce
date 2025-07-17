using MediatR;

namespace MiniCommerce.API.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
