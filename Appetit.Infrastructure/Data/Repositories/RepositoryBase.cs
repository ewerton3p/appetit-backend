using Appetit.Domain;
using Appetit.Domain.Common.Constants;
using Appetit.Domain.Common.Utils;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories;
public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
{
    public readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationDbContext _dbContext;

    public RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null)
    {
        var query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        if (order != null)
            query = query.OrderBy(order);

        return await query.AsNoTracking().ToListAsync();
    }

    public int Count(Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return query.Count();
    }

    public async Task<List<TEntity>> Get(IQueryable<TEntity> query, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null)
    {
        if (filter != null)
            query = query.Where(filter);

        if (order != null)
            query = query.OrderBy(order);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<PaginatedResult<TEntity>> GetWithPagination(int page, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null, bool descending = false)
    {
        var query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        var count = await query.CountAsync();

        if (order != null)
        {
            if (descending)
            {
                query = query.OrderByDescending(order);
            }
            else
            {
                query = query.OrderBy(order);
            }
        }

        query = query.Skip(PagingUtils.GetPageOffset(page))
            .Take(Paging.DefaultPageSize);

        var items = await query.AsNoTracking().ToListAsync();

        return new PaginatedResult<TEntity>
        {
            Items = items,
            TotalCount = count
        };
    }

    public async Task<PaginatedResult<TEntity>> GetWithPagination(IQueryable<TEntity> query, int page, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null, bool descending = false)
    {
        if (filter != null)
            query = query.Where(filter);

        var count = await query.CountAsync();

        if (order != null)
        {
            if (descending)
            {
                query = query.OrderByDescending(order);
            }
            else
            {
                query = query.OrderBy(order);
            }
        }

        query = query.Skip(PagingUtils.GetPageOffset(page))
            .Take(Paging.DefaultPageSize);

        var items = await query.AsNoTracking().ToListAsync();

        return new PaginatedResult<TEntity>
        {
            Items = items,
            TotalCount = count
        };
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task Update(TEntity entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}

public class PaginatedResult<TEntity>
{
    public List<TEntity>? Items { get; set; }
    public int TotalCount { get; set; }
}