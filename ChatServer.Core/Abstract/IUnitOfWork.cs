using Ardalis.Result;

namespace ChatServer.Core.Abstract;

public interface IUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
