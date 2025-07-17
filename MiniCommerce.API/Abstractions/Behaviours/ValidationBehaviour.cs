using FluentValidation;
using MediatR;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Abstractions.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }
        
        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(v => v.Validate(context))
            .Where(vr => vr.Errors.Any())
            .SelectMany(vr => vr.Errors)
            .Select(vf => new ValidationError(vf.PropertyName, vf.ErrorMessage))
            .ToList();

        if (validationErrors.Any())
        {
            throw new ValidationException(validationErrors);
        }
        
        return await next(cancellationToken);
    }
}

public record ValidationError(string PropertyName, string ErrorMessage);

public class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; set; }
    
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}