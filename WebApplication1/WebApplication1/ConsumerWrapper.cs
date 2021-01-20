using System;
using Confluent.Kafka;

namespace WebApplication1
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private IConsumer<string, string> _consumer;

        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            _topicName = topicName;
            _consumerConfig = config;
            _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

            _consumer.Subscribe(topicName);
        }
        public string ReadMessage()
        {
            try
            {
                var consumeResult = _consumer.Consume();
                return consumeResult.Message == null ? "" : consumeResult.Message.Value;
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
