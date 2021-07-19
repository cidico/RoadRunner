using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using RoadRunner.Config;
using RoadRunner.Publishers;

namespace RoadRunner
{
    public static class Publisher
    {
        public static ConcurrentDictionary<string, IQueuePublisher> WorkingPublishers;
        
        public static void Initialize(IConfiguration configuration)
        {
            ConfigLoader.LoadConfigs(configuration);
            
            if (PublisherFactory.Configuration.Enabled)
            {
                PublisherFactory.AddPublishers(PublisherFactory.Configuration.Publishers);    
            }
        }
        
        public static void Publish(string publisherName, string channelName, string message)
        {
            WorkingPublishers[publisherName].Publish(channelName, message);
        }
    }
}