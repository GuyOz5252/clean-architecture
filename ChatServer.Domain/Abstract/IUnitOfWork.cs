using Ardalis.Result;

namespace ChatServer.Domain.Abstract;

public interface IUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
