using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Asclepius.Auth.Business.Factories;

[Obsolete]
public class KafkaProducerFactory(IConfiguration configuration)
{
    public KafkaProducerService<T> Create<T>()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        var producer = new ProducerBuilder<T, string>(producerConfig).Build();
        return new KafkaProducerService<T>(producer);
    }
}