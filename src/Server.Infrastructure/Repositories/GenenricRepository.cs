using Server.Application.Common;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;

namespace Server.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected DbSet<TEntity> _dbSet;
    private readonly ICurrentTime _timeService;

    public GenericRepository(AppDbContext context, ICurrentTime timeService)
    {
        _dbSet = context.Set<TEntity>();
        _timeService = timeService;
    }
    public Task<List<TEntity>> GetAllAsync() => _dbSet.ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        // todo should throw exception when not found
        return result;
    }

    public async Task AddAsync(TEntity entity)
    {
        entity.CreationDate = _timeService.GetCurrentTime();
        await _dbSet.AddAsync(entity);
    }

    public void SoftRemove(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }

    public void Update(TEntity entity)
    {
        entity.ModificationDate = _timeService.GetCurrentTime();
        _dbSet.Update(entity);
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = _timeService.GetCurrentTime();
        }
        await _dbSet.AddRangeAsync(entities);
    }

    public void SoftRemoveRange(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = _timeService.GetCurrentTime();
        }
        _dbSet.UpdateRange(entities);
    }

    public async Task<Pagination<TEntity>> ToPagination(int pageIndex = 0, int pageSize = 10)
    {
        var itemCount = await _dbSet.CountAsync();
        var items = await _dbSet.OrderByDescending(x => x.CreationDate)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .ToListAsync();

        var result = new Pagination<TEntity>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items,
        };

        return result;
    }

    public void UpdateRange(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = _timeService.GetCurrentTime();
        }
        _dbSet.UpdateRange(entities);
    }
}

