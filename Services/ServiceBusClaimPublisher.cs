using Azure.Messaging.ServiceBus;
using Claims.Domain.Messages;
using System.Text.Json;

namespace Claims.Api.Services
{
    /// <summary>
    /// Provides functionality to publish claim messages to an Azure Service Bus queue.
    /// Concrete implementation of IClaimMessagePublisher using Azure Service Bus.
    /// Resolved via dependency injection.
    /// </summary>
    /// <remarks>This class is intended for use in scenarios where claim submission events need to be sent
    /// asynchronously to a Service Bus queue for further processing or integration with other systems. The
    /// configuration must supply valid Service Bus connection details. This class is not thread-safe for concurrent
    /// disposal or reconfiguration.</remarks>
    public class ServiceBusClaimPublisher : IClaimMessagePublisher
    {
        private readonly ServiceBusSender _sender;

        public ServiceBusClaimPublisher(IConfiguration configuration)
        {
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var queueName = configuration["ServiceBus:QueueName"];

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("ServiceBus connection string is missing.");

            if (string.IsNullOrWhiteSpace(queueName))
                throw new InvalidOperationException("ServiceBus queue name is missing.");

            var client = new ServiceBusClient(connectionString);
            _sender = client.CreateSender(queueName);
        }

        public async Task PublishClaimAsync(ClaimSubmittedMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            var sbMessage = new ServiceBusMessage(json);

            //For future: Use try-catch to handle potential exceptions during message sending, such as network issues or Service Bus errors.
            await _sender.SendMessageAsync(sbMessage);
        }
    }
}
