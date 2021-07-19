using System;
using System.Collections.Generic;

namespace RoadRunner.Config.Channels
{
    public class ChannelConfiguration
    {
        /// <summary>
        /// The channel's name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Indicates if the channel should be used.
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Channel's exchange name.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Channel's routing key.
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// Channel specific validations.
        /// </summary>
        public ChannelValidationConfiguration ChannelValidations { get; set; } = new ChannelValidationConfiguration();

        public IEnumerable<string> GetValidationErrors(int index)
        {
            var messages = new List<string>();
            
            if (this.Name.IsNullOrEmpty())
                messages.Add($"Channel's Name at position {index} cannot be empty.");

            if (this.ExchangeName.IsNullOrEmpty())
                messages.Add($"Channel's ExchangeName at position {index} cannot be empty.");
                
            if (this.RoutingKey.IsNullOrEmpty())
                messages.Add($"Channel's RoutingKey at position {index} cannot be empty.");

            return messages;
        }
    }
}