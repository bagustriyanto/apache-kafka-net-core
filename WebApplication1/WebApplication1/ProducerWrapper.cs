using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class ProducerWrapper
    {
        private string _topicName;
        private ProducerBuilder<string, string> _producer;
        private ProducerConfig _producerConfig;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _producerConfig = config;
            _producer = new ProducerBuilder<string, string>(_producerConfig);
        }

        public async Task WriteMessage(string message)
        {
            var producerResult = await _producer.Build().ProduceAsync(_topicName, new Message<string, string>() {
                Key = rand.Next(5).ToString(),
                Value = message
            });

            Console.WriteLine($"KAFKA => Delivered '{producerResult.Value}' to '{producerResult.TopicPartitionOffset}'");

            return;
        }
    }
}
