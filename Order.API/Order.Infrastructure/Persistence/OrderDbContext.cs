using Microsoft.EntityFrameworkCore;
using Order.Domain.Common;

namespace Order.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext, IUnitOfWork
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order.Domain.Entities.Order> Orders => Set<Order.Domain.Entities.Order>();

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Aquí puedes despachar eventos de dominio antes de guardar si usas DDD avanzado
            var result = await base.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}