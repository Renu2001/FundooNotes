using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utility
{
    public class KafkaService
    {
        private readonly IConfiguration _configuration;

        public KafkaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreateTopicAsync()
        {
            var topicName = _configuration["Kafka:TopicName"];
            var numPartitions = int.Parse(_configuration["Kafka:NumPartitions"]);
            var replicationFactor = short.Parse(_configuration["Kafka:ReplicationFactor"]);

            var config = new AdminClientConfig
            {
                BootstrapServers = _configuration["ProducerConfiguration:BootstrapServers"]
            };

            using var adminClient = new AdminClientBuilder(config).Build();
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification>
            {
                new TopicSpecification { Name = topicName, NumPartitions = numPartitions, ReplicationFactor = replicationFactor }
            });
                Console.WriteLine($"Topic {topicName} created successfully");
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.IsError)
                {
                    Console.WriteLine($"An error occurred creating topic {topicName}: {e.Results[0].Error.Reason}");
                }
                else
                {
                    Console.WriteLine($"Topic {topicName} already exists");
                }
            }
        }
    }
}

