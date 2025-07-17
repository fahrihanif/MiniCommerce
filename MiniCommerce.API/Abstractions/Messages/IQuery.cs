using MediatR;

namespace MiniCommerce.API.Abstractions.Messages;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;