using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using NLog;
using RoadRunner.Log;

namespace RoadRunner.Tests
{
    public class TestBase
    {
        protected const string PublisherName = "Publisher";
        protected const string Server = "localhost";
        protected const int Port = 5672;
        protected const string Username = "guest";
        protected const string Password = "guest";
        protected const string VirtualHost = "/";

        private Logger Logger { get; }

        protected TestBase()
        {
            Logger = RoadRunnerLogger.Instance.GetCurrentClassLogger();
        }

        protected IConfiguration BuildConfigurationWithoutRoadRunnerSection()
        {
            var settings = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ApplicationName", "RoadRunner"),
            };

            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);

            return cfgBuilder.Build();
        }

        protected IConfiguration BuildSettings(List<KeyValuePair<string, string>> settings)
        {
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);

            return cfgBuilder.Build();
        }

        protected IConfiguration BuildDefaultAppSettings()
        {
            var settings = this.CreateRootSection(
                enabled:true
                );
            
            settings.AddRange(this.CreateBasicPublisher());
            settings.AddRange(this.CreateBasicChannel(
                publisherIndex: 0, 
                channelName: "Channel1", 
                exchangeName: "exchange1",
                routingKey: "rk1")
            );

            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddInMemoryCollection(settings);

            return cfgBuilder.Build();
        }

        protected List<KeyValuePair<string, string>> CreateBasicPublisher(
            int publisherIndex = default,
            bool enabled = true,
            string publisherName = null,
            string server = null,
            int? port = null,
            string username = null,
            string password = null,
            string virutalHost = null
        )
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Enabled", $"{enabled}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Name",
                    $"{publisherName ?? PublisherName}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Server",
                    $"{server ?? Server}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Port", $"{(port ?? Port)}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Username", $"{username ?? Username}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Password", $"{password ?? Password}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:VirtualHost",
                    $"{virutalHost ?? VirtualHost}"),
            };
        }

        protected List<KeyValuePair<string, string>> CreateBasicChannel(
            int publisherIndex,
            string channelName,
            string exchangeName,
            string routingKey,
            bool channelEnabled = true,
            bool checkMessageSize = true,
            int maxMessageSize = 1024,
            int channelIndex = 0)
        {
            var settings = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    $"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:Enabled", $"{channelEnabled}"),
                new KeyValuePair<string, string>($"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:Name",
                    $"{channelName}"),
                new KeyValuePair<string, string>(
                    $"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:ExchangeName", $"{exchangeName}"),
                new KeyValuePair<string, string>(
                    $"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:RoutingKey", $"{routingKey}"),
                new KeyValuePair<string, string>(
                    $"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:Validations:CheckMessageSize",
                    $"{checkMessageSize}"),
                new KeyValuePair<string, string>(
                    $"RoadRunner:Publishers:{publisherIndex}:Channels:{channelIndex}:Validations:MaxMessageSize",
                    $"{maxMessageSize}"),
            };

            return settings;
        }

        protected List<KeyValuePair<string, string>> CreateRootSection(bool enabled = true)
        {
            var settings = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("RoadRunner:Enabled", $"{enabled}"),
            };

            return settings;
        }
    }
}