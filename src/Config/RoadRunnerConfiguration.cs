using System.Collections.Generic;
using System.Linq;
using RoadRunner.Config.Publishers;

namespace RoadRunner.Config
{
    public class RoadRunnerConfiguration
    {
        public bool Enabled { get; set; }
        
        public List<PublisherConfiguration> Publishers { get; set; } = new List<PublisherConfiguration>();

        public bool HasPublishers()
        {
            return this.Publishers.Any();
        }
    }
}