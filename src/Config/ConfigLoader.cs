using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using NLog;
using RoadRunner.Config.Publishers;
using RoadRunner.Log;
using RoadRunner.Publishers;

namespace RoadRunner.Config
{
    public static class ConfigLoader
    {
        private const string SectionName = "RoadRunner";

        private static IConfiguration AppConfiguration { get; set; }

        private static Logger logger = RoadRunnerLogger.Instance.GetCurrentClassLogger();

        private static RoadRunnerConfiguration Configuration { get; set; } = new RoadRunnerConfiguration();

        public static void LoadConfigs(IConfiguration configuration = null)
        {
            logger.Info("Starting the RoadRunner...");

            AppConfiguration = LoadConfiguration(configuration);

            if (!Configuration.Enabled)
            {
                var message = $"RoadRunner is off in your settings. Please check the RoadRunner:Enabled " +
                              $"configuration if you want to enable it.";

                logger.Warn(message);

                return;
            }

             

            ValidatePublishersConfigurations();

            PublisherFactory.SetConfiguration(Configuration);
        }

        private static IConfiguration LoadConfiguration(IConfiguration configuration)
        {
            if (configuration != null && configuration.GetSection(SectionName).Exists())
            {
                AppConfiguration = configuration;
            }
            else
            {
                try
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder();
                    builder.AddEnvironmentVariables();
                    builder.AddJsonFile("appsettings.json", false, false);
                    builder.AddJsonFile("appsettings.Development.json", true, false);
                    builder.AddJsonFile("appsettings.Staging.json", true, false);
                    builder.AddJsonFile("appsettings.Production.json", true, false);

                    AppConfiguration = builder.Build();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Configuration file not found or it's invalid.");
                    throw;
                }
            }

            if (AppConfiguration is null || (bool) !AppConfiguration?.GetSection(SectionName).Exists())
            {
                throw new InvalidOperationException("No RoadRunner section found. Please check your " +
                                                    "configuration file.");
            }

            logger.Info("RoadRunner section found.");

            try
            {
                AppConfiguration.GetSection(SectionName).Bind(Configuration);

                if (!Configuration.HasPublishers())
                    throw new InvalidOperationException("There are no publishers in your configuration file.");
                
                logger.Info($"Configuration successfully bound. Found {Configuration.Publishers.Count} publishers.");
            }
            catch (Exception ex)
            {
                var message = $"RoadRunner configuration section was not found or is invalid. RoadRunner " +
                              $"is disabled. Please check your settings if you want to enable it.";

                logger.Error(ex, message);
            }

            return AppConfiguration;
        }

        private static void ValidatePublishersConfigurations()
        {
            for (var index = 0; index < Configuration.Publishers.Count; index++)
            {
                PublisherConfiguration pubConfig = Configuration.Publishers[index];

                IEnumerable<string> validationResult = pubConfig.GetValidationErrors(index).ToArray();

                if (validationResult.Any())
                {
                    var messages = new StringBuilder();

                    foreach (var result in validationResult)
                    {
                        logger.Fatal($"Invalid configuration for publisher at position {index}. Errors: {result}");
                        messages.AppendLine(result);
                    }

                    throw new InvalidOperationException(messages.ToString());
                }
            }
        }
    }
}