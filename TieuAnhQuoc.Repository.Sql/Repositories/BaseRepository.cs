using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TieuAnhQuoc.ObjectMapper;
using TieuAnhQuoc.Repository.Sql.Entities;
using TieuAnhQuoc.Repository.Sql.Models;

namespace TieuAnhQuoc.Repository.Sql.Repositories;

public interface IBaseRepository<TEntity, TIdType> where TEntity : BaseEntity<TIdType>
{
    public Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        string includeProperties,
        int skip,
        int limit);

    public Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        string includeProperties);

    public Task<List<TDto>> FindAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        string includeProperties);

    public Task<List<TDto>> FindAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        string includeProperties,
        int skip,
        int limit);

    public Task<FindResult<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit,
        string includeProperties = null);

    public Task<FindResult<TDto>> FindAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit,
        string includeProperties = null);

    public Task<TEntity> FindOneAsync(TIdType id);

    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null,
        string includeProperties = null);

    public Task<TDto> FindOneAsync<TDto>(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null,
        string includeProperties = null);

    public Task<bool> InsertAsync(TEntity entity);
    public Task<bool> InsertAsync(IEnumerable<TEntity> entities);
    public Task<bool> UpdateAsync(TEntity entity);
    public Task<bool> UpdateAsync(IEnumerable<TEntity> entities);
    public Task<bool> DeleteAsync(TEntity entity);
    public Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
}

public class BaseRepository<TEntity, TIdType> : IBaseRepository<TEntity, TIdType> where TEntity : BaseEntity<TIdType>
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly DbContext _context;

    public BaseRepository(DbContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        string includeProperties,
        int skip,
        int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        return await query.ToListAsync();
    }

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        string includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        return await query.ToListAsync();
    }

    public async Task<List<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        string includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        var result = await query.ToListAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<List<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        string includeProperties, int skip, int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);


        var result = await query.ToListAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<FindResult<TEntity>> FindAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        int skip, int limit,
        string includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }


        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        var items = await query.ToListAsync();
        long totalCount = await query.CountAsync();
        return FindResult<TEntity>.Success(items, totalCount);
    }

    public async Task<FindResult<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        int skip,
        int limit,
        string includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        var result = await query.ToListAsync();
        long totalCount = await query.CountAsync();
        return FindResult<TDto>.Success(result.ProjectTo<TEntity, TDto>(), totalCount);
    }

    public async Task<TEntity> FindOneAsync(TIdType id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null, string includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TDto> FindOneAsync<TDto>(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null, string includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByProps = orderBy.Split(',');
            foreach (var orderByProp in orderByProps)
            {
                var propertyName = orderByProp.Split(' ')[0];
                var propType = typeof(TEntity).GetProperty(propertyName);
                if (!string.IsNullOrEmpty(propertyName) && propType != null)
                {
                    query = orderByProp.Contains("desc")
                        ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
                        : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
                }
            }
        }

        var result = await query.FirstOrDefaultAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> InsertAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> DeleteAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _context.SaveChangesAsync() >= 1;
    }
}