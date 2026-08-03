using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Order.Application.Common.Interfaces;

namespace Order.Infrastructure.Messaging
{
    public class KafkaEventProducer : IEventProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventProducer(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string topic, string key, T message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            
            var kafkaMessage = new Message<string, string>
            {
                Key = key,
                Value = jsonMessage
            };

            await _producer.ProduceAsync(topic, kafkaMessage);
        }
    }
}