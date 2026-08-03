using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Messaging
{
    public class KafkaEventConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public KafkaEventConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = "order-readmodel-sync-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("order-created");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult != null)
                    {
                        var eventData = JsonSerializer.Deserialize<OrderCreatedEventDto>(consumeResult.Message.Value);

                        if (eventData != null)
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                            // Evitar duplicados si el evento se procesa más de una vez (Idempotencia básica)
                            var exists = await dbContext.OrderReadModels.FindAsync(new object[] { eventData.OrderId }, stoppingToken);
                            if (exists == null)
                            {
                                var readModel = new OrderReadModel
                                {
                                    Id = eventData.OrderId,
                                    CustomerId = eventData.CustomerId,
                                    TotalAmount = eventData.Items.Sum(i => i.Quantity * i.UnitPrice),
                                    TotalItems = eventData.Items.Sum(i => i.Quantity),
                                    CreatedAt = eventData.CreatedAt
                                };

                                dbContext.OrderReadModels.Add(readModel);
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }

    public record OrderCreatedEventDto(Guid OrderId, string CustomerId, DateTime CreatedAt, List<OrderItemEventDto> Items);
    public record OrderItemEventDto(string ProductId, int Quantity, decimal UnitPrice);
}