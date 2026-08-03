namespace Order.Application.Common.Interfaces
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic, string key, T message);
    }
}