using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using RabbitMQ.Client;
using RoadRunner.Config;
using RoadRunner.Config.Publishers;
using RoadRunner.Log;

namespace RoadRunner.Publishers
{
    internal static class PublisherFactory
    {
        private static Logger logger = RoadRunnerLogger.Instance.GetCurrentClassLogger();

        public static RoadRunnerConfiguration Configuration { get; private set; }

        static PublisherFactory() => Publisher.WorkingPublishers = new ConcurrentDictionary<string, IQueuePublisher>();

        public static void SetConfiguration(RoadRunnerConfiguration config) => Configuration = config;

        internal static void AddPublishers(List<PublisherConfiguration> publishersConfigurations)
        {
            logger.Info($"Found {publishersConfigurations.Count} publishers.");
            
            foreach (PublisherConfiguration publisherConfig in publishersConfigurations)
            {
                if (!publisherConfig.Enabled)
                {
                    logger.Warn($"Publisher {publisherConfig.Name} is disabled.");
                    continue;
                }
                
                logger.Info($"Publisher {publisherConfig.Name} is enabed.");
                ConnectionFactory factory = BuildConnectionFactory(publisherConfig);
                logger.Info($"Connection factory for publisher {publisherConfig.Name} built.");

                IQueuePublisher publisher = new QueuePublisher(publisherConfig, factory);
                publisher.InitializeRabbitMqConnection();

                if (!Publisher.WorkingPublishers.ContainsKey(publisherConfig.Name))
                {
                    Publisher.WorkingPublishers.TryAdd(publisherConfig.Name, publisher);
                    logger.Info($"Publisher {publisherConfig.Name} added to the list of available " +
                                $"publishers and it's ready to work.");
                }
            }
        }
  
        private static ConnectionFactory BuildConnectionFactory(PublisherConfiguration publisherConfiguration)
        {
            Assembly asm = Assembly.GetEntryAssembly();
            
            var name = $"{Environment.MachineName}-{asm?.GetName().Name}-" +
                       $"v{asm?.GetName().Version}-{publisherConfiguration.Name}";

            return new ConnectionFactory
            {
                HostName = publisherConfiguration.Server,
                UserName = publisherConfiguration.Username,
                Password = publisherConfiguration.Password,
                Port = publisherConfiguration.Port,
                VirtualHost = publisherConfiguration.VirtualHost,
                ClientProvidedName = name
            };
        }
    }
}