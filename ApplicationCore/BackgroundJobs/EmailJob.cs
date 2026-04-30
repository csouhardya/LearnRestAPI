using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text.Json;

namespace ApplicationCore.BackgroundJobs
{
    public class EmailJob : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ConsumerConfig _config;
        private readonly IEmailService _emailService;
        public EmailJob(ILogger logger, IEmailService emailService, IConfiguration configuration)
        {
            _config = new()
            {
                BootstrapServers = configuration[Constants.KafkaSeverKey], // TODO: Inject in middleware context writer
                GroupId = Constants.EmailConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config).Build();
            consumer.Subscribe(Constants.ProduceOrderTopic);
            _logger.Information("Subscription successfull to order topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var orders = JsonSerializer.Deserialize<List<Order>>(result.Message.Value);

                    if (orders != null && orders.Count > 1)
                    {
                        _logger.Information(
                            $"Sending email to [{string.Join(",", orders.Select(x => x.EmailAddress).Distinct())}] " +
                            $"for order id: [{string.Join(",", orders.Select(x => x.guid).Distinct())}]");
                        await _emailService.SendEmailAsync(orders);
                    }
                    _logger.Warning("No order avaiable to send email");
                }
                catch(ConsumeException cex)
                {
                    _logger.Error($"Something went wrong with email consumer. {cex}");
                }
                catch (Exception ex)
                {
                    _logger.Fatal($"Something went wrong. Unable to send stream to email. Ex: {ex}");
                }
            }
        }
    }
}
