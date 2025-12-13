using Ardalis.Result;

namespace ChatServer.Application.Messaging;

public interface ICommand : Mediator.ICommand<Result>;

public interface ICommand<T> : Mediator.ICommand<Result<T>>;
