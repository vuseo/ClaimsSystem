using Claims.Domain.Messages;

namespace Claims.Api.Services
{
    public interface IClaimMessagePublisher
    {
        /// <summary>
        /// Publishes a claim submission message to the processing system asynchronously.
        /// </summary>
        /// <param name="message">The claim submission message to be published. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task PublishClaimAsync(ClaimSubmittedMessage message);
    }
}
