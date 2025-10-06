namespace Asclepius.Auth.Domain.Interfaces;

public interface IKafkaProducer : IDisposable
{
    Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);
}