using Microsoft.EntityFrameworkCore;
using Order.Domain.Common;

namespace Order.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext, IUnitOfWork
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order.Domain.Entities.Order> Orders => Set<Order.Domain.Entities.Order>();

        public DbSet<OrderReadModel> OrderReadModels => Set<OrderReadModel>();

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Aquí puedes despachar eventos de dominio antes de guardar si usas DDD avanzado
            var result = await base.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }

    public class OrderReadModel
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}