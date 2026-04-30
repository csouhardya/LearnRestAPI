using ApplicationCore.Interfaces;
using Confluent.Kafka;
using ApplicationCore.Misc;
using Serilog;
using ApplicationCore.Models;
using System.Text.Json;

namespace ApplicationCore.Services
{
    public class KafkaService(ILogger logger, IProducer<string, string> producer, ProducerConfig config) : IKafkaService
    {
        private readonly ILogger _logger = logger;
        private readonly IProducer<string, string> _producer = producer;
        private readonly ProducerConfig _config = config;
        public async Task ProduceOrderAsync(List<Order> orders)
        {
            try
            {
                _logger.Information($"Producer server: {_config.BootstrapServers}, producer topic: {Constants.ProduceOrderTopic}");
                var deliveryResult = await _producer.ProduceAsync(topic: Constants.ProduceOrderTopic, new Message<string, string>
                {
                    Key = Constants.ProductOrderMessageKey,
                    Value = JsonSerializer.Serialize(orders)
                });
                _logger.Information($"Sent item: {deliveryResult.Value}, Offset: {deliveryResult.Offset}, TopicPartition {deliveryResult.TopicPartition}, TopicPartitionOffset: {deliveryResult.TopicPartitionOffset}, PartitionOffset: {deliveryResult.Partition}");
            }
            catch(Exception ex)
            {
                _logger.Error($"Deliver failed. Message: {0}", ex.Message);
            }
        }
    }
}
