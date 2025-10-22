using Ardalis.Result;

namespace Flightware.Application;

public interface ICommandHandler<in TCommand> : Mediator.ICommandHandler<TCommand, Result>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResult> : Mediator.ICommandHandler<TCommand, Result<TResult>>
    where TCommand : ICommand<TResult>;
