using Ardalis.Result;

namespace Flightware.Application.Messaging;

public interface ICommand : Mediator.ICommand<Result>;

public interface ICommand<T> : Mediator.ICommand<Result<T>>;
