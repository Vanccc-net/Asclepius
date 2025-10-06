using Asclepius.Auth.Domain.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Asclepius.Auth.Business;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    private bool _disposed;

    public KafkaProducer(IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(topic, new Message<string, string> { Value = message }, cancellationToken);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _producer.Flush(TimeSpan.FromSeconds(10));
                _producer.Dispose();
            }

            _disposed = true;
        }
    }
}