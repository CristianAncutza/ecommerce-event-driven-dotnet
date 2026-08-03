using MediatR;

namespace Order.Application.Features.Orders.Commands.CreateOrder
{
    // El comando representa la intención y lleva los datos de entrada
    public record CreateOrderCommand(
        string CustomerId,
        List<OrderItemDto> Items
    ) : IRequest<Guid>;

    public record OrderItemDto(
        string ProductId,
        int Quantity,
        decimal UnitPrice
    );
}