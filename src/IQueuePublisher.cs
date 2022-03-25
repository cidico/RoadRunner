using System.Collections.Concurrent;
using RabbitMQ.Client;
using RoadRunner.Config.Channels;

namespace RoadRunner
{
    public interface IQueuePublisher
    {
        /// <summary>
        /// A list of channels available for this publisher.
        /// </summary>
        ConcurrentDictionary<ChannelIdentifier, IModel> Channels { get; set; }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        void PublishAsync(string channelName, string message);

        /// <summary>
        /// Initializes the publisher connection with RabbitMQ defined in the configuration file.
        /// </summary>
        void InitializeRabbitMqConnection();
    }
}