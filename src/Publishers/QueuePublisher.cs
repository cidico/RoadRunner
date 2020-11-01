using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using RabbitMQ.Client;
using RoadRunner.Config.Channels;
using RoadRunner.Config.Publishers;
using RoadRunner.Log;

namespace RoadRunner.Publishers
{
    /// <summary>
    /// Publishes messages to a RabbitMQ connection.
    /// </summary>
    internal class QueuePublisher : IQueuePublisher
    {
        private static Logger logger = RoadRunnerLogger.Instance.GetCurrentClassLogger();

        private ConnectionFactory Factory { get; set; }

        private PublisherConfiguration Configuration { get; set; }

        private IConnection Connection { get; set; }

        public ConcurrentDictionary<ChannelIdentifier, IModel> Channels { get; set; } = new ConcurrentDictionary<ChannelIdentifier, IModel>();

        public QueuePublisher(PublisherConfiguration config, ConnectionFactory factory)
        {
            this.Configuration = config;
            this.Factory = factory;
        }

        public void InitializeRabbitMqConnection()
        {
            try
            {
                this.Connection = Factory.CreateConnection();
                logger.Info($"Connection successfully started for Publisher '{this.Configuration.Name}'.");
                this.InitializeChannels();
            }
            catch (Exception ex)
            {
                string message = BuildConnectionErrorMessage(Configuration);
                logger.Error(ex, message);
            }
        }

        private void InitializeChannels()
        {
            var enabledChannels = GetEnabledChannels();
            var disabledChannels = GetDisabledChannels();
            
            foreach (var channelConfig in disabledChannels)
            {
                logger.Warn($"Channel {channelConfig.Name} is disabled. If you want to enabled it change " +
                            $"your settings at RoadRunner:Publishers[{this.Configuration.Name}]" +
                            $":Channels[{channelConfig.Name}]:Enabled.");
            }
            
            foreach (var channelConfig in enabledChannels)
            {
                try
                {
                    var channel = this.CreateModel(channelConfig, out ChannelIdentifier key);
                    this.Channels.TryAdd(key, channel);

                    logger.Info($"Channel successfully created for channel '{channelConfig.Name}' and it " +
                                $"is ready to work.");
                }
                catch (Exception ex)
                {
                    var message = $"The publisher {channelConfig.Name} is enabled but there's something wrong " +
                                  $"with the configuration. " +
                                  $"Please check your settings and try again.";
                        
                    logger.Error(ex, message);
                }
            }
        }

        private IEnumerable<ChannelConfiguration> GetDisabledChannels()
        {
            return this.Configuration.Channels
                .Where(x => !x.Enabled);
        }

        private IEnumerable<ChannelConfiguration> GetEnabledChannels()
        {
            return this.Configuration.Channels
                .Where(x => x.Enabled);
        }

        private ChannelConfiguration GetChannelConfiguration(string channelName)
        {
            return this.Configuration.Channels.First(x => x.Name == channelName);
        }
        
        private IModel CreateModel(ChannelConfiguration config, out ChannelIdentifier key)
        {
            var channel = this.Connection.CreateModel();

            key = new ChannelIdentifier(config.Name, channel.ToString());
            
            return channel;
        }

        public void Publish(string channelName, string message)
        {
            var channel = this.Channels.FirstOrDefault(
                x => x.Key.ChannelName.Equals(channelName, StringComparison.InvariantCultureIgnoreCase)
            ).Value;
            
            if(channel is null)
                throw new InvalidOperationException($"");
            
            var channelConfig = this.Configuration.Channels
                .FirstOrDefault(x => x.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase));
            
            AssertCanSendMessage(channelConfig, message);
            
            if (this.Connection.IsOpen && channel.IsOpen)
            {
                Span<byte> messageBytes = message.ToBytes();

                if (channelConfig.ChannelValidations.CheckMessageSize && messageBytes.Length > channelConfig.ChannelValidations.MaxMessageSize)
                {
                    throw new ArgumentOutOfRangeException($"Message size check IS ENABLED and " +
                                                          $"it exceeds the MaxMessageSize property defined for the " +
                                                          $"channel {channelConfig.Name}.");
                }
                
                // // TODO: add this settings in the config file ?
                // if(channelConfig.ForceExchangeCreation) 
                //     channel.ExchangeDeclare(channelConfig.ExchangeName, "fanout", true, false, null);
                //
                // channel.bind;
                
                channel.BasicPublish(
                    exchange: channelConfig.ExchangeName, 
                    routingKey: channelConfig.RoutingKey,
                    basicProperties: null, 
                    body: message.ToBytes()
                );
            }
        }

        private void AssertCanSendMessage(ChannelConfiguration channelConfig, string message)
        {
            if (!this.Configuration.Enabled)
                throw new InvalidOperationException($"Publisher {Configuration.Name} is not enabled. " +
                                                    $"Please check your configuration file.");

            

            if (channelConfig is null)
                throw new ArgumentException($"Channel {channelConfig.Name} for publisher {Configuration.Name} " +
                                            $"not found in configuration file.");

            if (!channelConfig.Enabled)
                throw new InvalidOperationException($"Channel {channelConfig.Name} " +
                                                    $"for publisher {Configuration.Name} is not enabled. " +
                                                    $"Please check your configuration file.");

            if (message.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(message));
        }
        
        private string BuildConnectionErrorMessage(PublisherConfiguration publisherConfiguration)
        {
            var message = new StringBuilder();
            message.AppendLine($"Fail to connect to the destination.");
            message.AppendLine($"Server: {publisherConfiguration.Server}");
            message.AppendLine($"Port: {publisherConfiguration.Port}");
            message.AppendLine($"Username: {publisherConfiguration.Username}");
            message.AppendLine($"VirtualHost: {publisherConfiguration.VirtualHost}");

            return message.ToString();
        }
    }
}