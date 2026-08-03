using Order.Domain.Common;

namespace Order.Domain.Repositories;

public interface IOrderRepository
{
    Task<Entities.Order?> GetByIdAsync(Guid id);
    Task AddAsync(Entities.Order order);
    Task UpdateAsync(Entities.Order order);
    Task SaveChangesAsync();

    IUnitOfWork UnitOfWork { get; }
}