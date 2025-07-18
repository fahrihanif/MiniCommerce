using MediatR;

namespace MiniCommerce.API.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;