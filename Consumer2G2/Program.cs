using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Consumer2G2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string bootstrapServers = "localhost:9092"; // Replace with your Kafka bootstrap servers
            string groupId = "group2"; // Consumer group ID
            string topic = "KAFKADEMO4"; // Replace with your Kafka topic name

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest, // Set offset to beginning for this example; adjust as needed
                EnableAutoCommit = true // Enable auto commit of offsets
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Assign(new List<TopicPartitionOffset>
            {
                new TopicPartitionOffset(topic, 1, Offset.Beginning) // Partition 1 from the beginning
            });

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // Prevent the process from terminating.
                cancellationTokenSource.Cancel();
                Console.WriteLine("Exiting...");
            };
            
            try
            {
                while (true)
                {
                    Console.WriteLine("c2g2");
                    var consumeResult = consumer.Consume(cancellationTokenSource.Token);
                    Console.WriteLine($"Partition 1 - Received message: {consumeResult.Message.Value}");

                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested.
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
