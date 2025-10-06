using System.Text.Json;
using Confluent.Kafka;

namespace Asclepius.Auth.Business;

public class KafkaProducerService<TK>(IProducer<TK, string> producer) : IDisposable
{
    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
    }

    public async Task ProduceAsync<T>(string topic, T message) where T : class
    {
        var serializedValue = JsonSerializer.Serialize<T>(message);
        await producer.ProduceAsync(topic, new Message<TK, string> { Value = serializedValue });
    }

    public async Task ProduceAsync(string topic, string message)
    {
        await producer.ProduceAsync(topic, new Message<TK, string> { Value = message });
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                producer.Flush(TimeSpan.FromSeconds(10));
                producer.Dispose();
            }

            _disposed = true;
        }
    }
}