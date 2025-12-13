using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mediator;

namespace ChatServer.Application.Pipeline;

public class ValidationPipelineBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : class, IResult
{
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(message, cancellationToken);
        }
        
        var validationResults = await Task.WhenAll(_validators
            .Select(validator => validator.ValidateAsync(message, cancellationToken)));

        if (validationResults.All(validationResult => validationResult.IsValid))
        {
            return await next(message, cancellationToken);
        }
        
        var validationErrors = validationResults
            .SelectMany(validationResult => validationResult.AsErrors())
            .DistinctBy(validationError => new { validationError.Identifier, validationError.ErrorMessage })
            .ToList();

        if (typeof(TResponse) == typeof(Result))
        {
            return Result.Invalid(validationErrors) as TResponse;
        }
            
        var result = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
            .GetMethod(nameof(Result.Invalid), [typeof(IEnumerable<ValidationError>)])!
            .Invoke(null, [validationErrors])!;
            
        return result as TResponse;

    }
}
