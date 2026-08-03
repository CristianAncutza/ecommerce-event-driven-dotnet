using Order.Domain.Common;
using Order.Domain.Repositories;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        // El repositorio expone el contexto como IUnitOfWork
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order.Domain.Entities.Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task AddAsync(Order.Domain.Entities.Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public Task UpdateAsync(Domain.Entities.Order order)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}