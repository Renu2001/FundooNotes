using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utility
{
    public class KafkaProducer
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;
        private readonly string _topicName;

        public KafkaProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            _producerConfig = new ProducerConfig();
            _configuration.Bind("ProducerConfiguration", _producerConfig);
            _topicName = _configuration["Kafka:TopicName"];
        }

        public async Task ProduceMessageAsync<T>(T message, int partitionKey)
        {
            using var producer = new ProducerBuilder<int, string>(_producerConfig).Build();
            var serializedMessage = JsonConvert.SerializeObject(message);

            try
            {
                var result = await producer.ProduceAsync(
                    new TopicPartition(_topicName, new Partition(partitionKey)),
                    new Message<int, string> { Key = partitionKey, Value = serializedMessage });

                Console.WriteLine($"Message '{serializedMessage}' delivered to partition {result.Partition} with offset {result.Offset}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Error producing message: {e.Error.Reason}");
            }
        }
    }
}