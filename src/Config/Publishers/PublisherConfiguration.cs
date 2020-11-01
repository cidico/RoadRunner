using System;
using System.Collections.Generic;
using RoadRunner.Config.Channels;

namespace RoadRunner.Config.Publishers
{
    public class PublisherConfiguration
    {
        public bool Enabled { get; set; }

        public string Name { get; set; }
        
        /// <summary>
        /// The server name or IP.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// The username to be used to authenticate in the server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to be used to authenticate in the 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The server's port to be used. Default value is 5672.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The user virtual host.
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        public List<ChannelConfiguration> Channels { get; set; } = new List<ChannelConfiguration>();

        public IEnumerable<string> GetValidationErrors(int index)
        {
            var messages = new List<string>();

            if (this.Name.IsNullOrEmpty())
                messages.Add($"Publisher's Name at position {index} cannot be empty.");

            if (this.Server.IsNullOrEmpty())
                messages.Add($"Publisher's Server at position {index} cannot be empty.");
                
            if (this.Port == default)
                messages.Add($"Publisher's Port at position {index} cannot be empty.");
                
            if (this.VirtualHost.IsNullOrEmpty())
                messages.Add($"Publisher's VirtualHost at position {index} cannot be empty.");
                
            if (this.Username.IsNullOrEmpty())
                messages.Add($"Publisher's Username at position {index} cannot be empty.");
                
            if (this.Password.IsNullOrEmpty())
                messages.Add($"Publisher's Password at position {index} cannot be empty.");
            
            return messages;
        }
    }
}