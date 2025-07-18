using System.Diagnostics.CodeAnalysis;

namespace MiniCommerce.API.Abstractions.Messages;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }
        
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
        
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    
    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    
    public Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        _value = value;
    }
    
    [NotNull]
    public TValue? Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("The value of failure result can not be accessed.");
    
    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
}