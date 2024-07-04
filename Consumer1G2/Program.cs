using Confluent.Kafka;

namespace Consumer1G2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string bootstrapServers = "localhost:9092"; // Replace with your Kafka bootstrap servers
            string groupId = "group2"; // Consumer group ID
            string topic = "KAFKADEMO4"; // Replace with your Kafka topic name
            int id = 0;
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
                new TopicPartitionOffset(topic, 0, Offset.Beginning) // Partition 0 from the beginning
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
                    Console.WriteLine("c1g2");
                    var consumeResult = consumer.Consume(cancellationTokenSource.Token);
                    Console.WriteLine($"Partition 0 - Received message: {consumeResult.Message.Value}");

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
