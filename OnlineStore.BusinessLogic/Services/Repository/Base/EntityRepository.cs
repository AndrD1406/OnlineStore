using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository.Base;

public class EntityRepository <TEntity> where TEntity : class
{
    public readonly OnlineStoreDbContext dbContext;
    public readonly DbSet<TEntity> dbSet;

    protected EntityRepository(OnlineStoreDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = this.dbContext.Set<TEntity>();
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        await dbSet.AddAsync(entity).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return await Task.FromResult(entity).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities)
    {
        await dbSet.AddRangeAsync(entities).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return await Task.FromResult(entities).ConfigureAwait(false);
    }

    public async Task<T> RunInTransaction<T>(Func<Task<T>> operation)
    {
        var executionStrategy = dbContext.Database.CreateExecutionStrategy();

        return await executionStrategy.ExecuteAsync(
            async () =>
            {
                await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    var result = await operation().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            });
    }

    public async Task RunInTransaction(Func<Task> operation)
    {
        var executionStrategy = dbContext.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(
            async () =>
            {
                await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await operation().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            });
    }

    public async Task Delete(TEntity entity)
    {
        dbContext.Entry(entity).State = EntityState.Deleted;

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await dbSet.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> GetAllWithDetails(string includeProperties = "")
        => await dbSet
        .IncludeProperties(includeProperties)
        .ToListAsync();

    public async Task<IEnumerable<TEntity>> GetByFilter(
        Expression<Func<TEntity, bool>> whereExpression,
        string includeProperties = "")
        => await this.dbSet
        .Where(whereExpression)
        .IncludeProperties(includeProperties)
        .ToListAsync()
        .ConfigureAwait(false);


}
