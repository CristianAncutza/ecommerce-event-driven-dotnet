using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Orders.Commands.CreateOrder;

namespace Order.API.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/orders")
                              .WithTags("Orders");

            // Endpoint para crear una orden
            group.MapPost("/", async (CreateOrderCommand command, ISender sender) =>
            {
                try
                {
                    // Envía el comando a través de MediatR al Handler correspondiente
                    var orderId = await sender.Send(command);
                    
                    return Results.Created($"/api/orders/{orderId}", new { OrderId = orderId });
                }
                catch (Exception ex)
                {
                    // Manejo básico de errores (puedes robustecerlo con un Global Exception Handler)
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("CreateOrder")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests);
        }
    }
}