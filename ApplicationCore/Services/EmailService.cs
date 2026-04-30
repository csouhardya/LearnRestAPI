using ApplicationCore.Interfaces;
using ApplicationCore.Misc;
using ApplicationCore.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Serilog;

namespace ApplicationCore.Services
{
    public class EmailService(ILogger logger, IConfiguration configuration) : IEmailService
    {
        private ILogger _logger = logger;
        private IConfiguration _configuration = configuration;
        public async Task SendEmailAsync(List<Order> orders)
        {
            try
            {
                // TODO : inject in middleware context writer
                var adminEmail = _configuration[Constants.EmailAddressKey] ?? throw new InvalidDataException("No email key found");
                var adminPassword = _configuration[Constants.EmailPasswordKey] ?? throw new InvalidDataException("No email password key found");
                var orderId = orders.Select(_ => _.guid).Distinct().First();
                var orderEmailAddress = orders.Select(_ => _.EmailAddress).Distinct().First();
                var orderUsername = orders.Select(_ => _.Username).Distinct().First();
                var orderDetails = string.Join(
                    Environment.NewLine,
                    orders.Select(_ => $"Product: {_.ProductName} -> Quantity: {_.Count} -> Amount: ->  {_.TotalAmount}"));

                var email = new MimeMessage();
                email.To.Add(MailboxAddress.Parse(orderEmailAddress)); // as all the order will belong to one customer
                _logger.Information($"Email receipient added successfully. Receipient: {orderEmailAddress}");

                email.From.Add(MailboxAddress.Parse(adminEmail));
                _logger.Information($"Email sender address added successfully. Sender: {adminEmail}");

                email.Subject = $"Order {orderId} placed successfully";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = $"""
                        Hi {orderUsername},
                    
                        Thanks for shopping with us.
                        You order has been placed successfully.
                        Order details:
                        Order ID : {orderId}
                        Order Details: 
                        {orderDetails}

                        Thanks,
                        BuildEcom
                    
                    """
                };

                _logger.Information("Connecting smtp client");
                using var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                _logger.Information("Connection established to smtp.gmail.com");

                await smtpClient.AuthenticateAsync(adminEmail,adminPassword);
                _logger.Information("Authentication sender");

                await smtpClient.SendAsync(email);
                _logger.Information($"Email sent successfully for order {orderId} confirmation");

                await smtpClient.DisconnectAsync(true);
            }
            catch(Exception ex)
            {
                _logger.Fatal($"Could not send email due to exception: {ex}");
            }
        }
    }
}
