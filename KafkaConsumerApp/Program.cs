using Confluent.Kafka;

namespace KafkaConsumerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                GroupId = "gid-consumers",
                BootstrapServers = "localhost:9092"
            };

            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
            {
                consumer.Subscribe("testdata");
                while (true)
                {
                    var bookingDetails = consumer.Consume();
                    Console.WriteLine(bookingDetails.Message.Value);
                }
            }
        }
    }
}
