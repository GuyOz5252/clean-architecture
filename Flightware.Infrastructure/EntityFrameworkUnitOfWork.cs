using Ardalis.Result;
using Common.Core.Abstract;

namespace Flightware.Infrastructure;

public class EntityFrameworkUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public EntityFrameworkUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            // TODO: Log
            return Result.Error();
        }

        return Result.Success();
    }
}
