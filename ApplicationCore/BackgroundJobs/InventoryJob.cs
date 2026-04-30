using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text.Json;

namespace ApplicationCore.BackgroundJobs
{
    public class InventoryJob : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public InventoryJob(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, ILogger logger)
        {

            _config = new ConsumerConfig()
            {
                BootstrapServers = configuration[Constants.KafkaSeverKey], // Kafka broker address consumer connects to. // TODO: Inject in middleware context writer
                GroupId = Constants.InventoryConsumerGroupId, // Consumer group identifier.
                AutoOffsetReset = AutoOffsetReset.Earliest, // Determines where consumer starts reading if no committed offset exists. Earliest -> start from the beginning
                EnableAutoCommit = true  // Kafka automatically stores latest consumed offsets periodically.
            };
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.Information("InventoryJob Execution started");
            using var consumer = new ConsumerBuilder<string, string>(_config)
                .Build();
            consumer.Subscribe(Constants.ProduceOrderTopic);

            _logger.Information("Subscription successful to kafka topic");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result == null)
                    {
                        continue;
                    }
                    var orders = JsonSerializer.Deserialize<List<Order>>(result.Message.Value);

                    _logger.Information("Json derialization successful");
                    if (orders != null && orders.Count > 0)
                    {
                        var invLst = new List<Inventory>();
                        foreach (var item in orders)
                        {
                            var inv = new Inventory() { CountToSubtract = item.Count, ProductGuid = item.ProductGuid };
                            invLst.Add(inv);
                        }
                        _logger.Information($"Updating Inventory for products:[{string.Join(",", invLst.Select(_ => _.ProductGuid).Distinct())}]");

                        // As BackgroundService is singleton and inventoryService and inventoryRepository is scoped.
                        using var scope = _serviceScopeFactory.CreateScope();
                        var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();
                        await inventoryService.UpdateInventoriesAsync(invLst);
                    }
                    _logger.Warning($"No order available to update");
                }
                catch (ConsumeException cex)
                {
                    _logger.Error($"Something went wrong with inventory consumer. {cex}");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Something went wrong. Ex: {ex}");
                }
            }

        }
    }
}
