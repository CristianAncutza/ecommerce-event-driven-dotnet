using MediatR;
using Order.Application.Common.Interfaces;
using Order.Domain.Repositories;

namespace Order.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventProducer _eventProducer;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventProducer eventProducer)
        {
            _orderRepository = orderRepository;
            _eventProducer = eventProducer;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Crear entidad y añadir items
            var order = Order.Domain.Entities.Order.Create(request.CustomerId);

            foreach (var item in request.Items)
            {
                order.AddItem(item.ProductId, item.Quantity, (int)item.UnitPrice);
            }

            // 2. Persistir en la base de datos transaccional (OLTP)
            await _orderRepository.AddAsync(order);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // 3. Publicar evento a Kafka de forma asíncrona
            var orderCreatedEvent = new
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items
            };

            await _eventProducer.ProduceAsync("order-created", order.Id.ToString(), orderCreatedEvent);

            return order.Id;
        }
    }
}