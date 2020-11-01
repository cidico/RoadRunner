using System.Collections.Generic;
using System.Linq;
using RoadRunner.Config.Publishers;
using Xunit;

namespace RoadRunner.Tests.Config.Publishers
{
    public class PublisherConfigurationTests
    {
        [Fact]
        public void Configuration_WithValidData_ShouldNotAddError()
        {
            var config = new PublisherConfiguration
            {
                Enabled = true,
                Name = "name",
                Server = "server",
                Username = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672
            };

            IEnumerable<string> result = config.GetValidationErrors(0);
            
            Assert.True(!result.Any());
        }
        
        [Fact]
        public void Configuration_WithInvalidData_ShouldAddError()
        {
            var config = new PublisherConfiguration
            {
                Enabled = true,
                Name = "",
                Server = "",
                Username = "",
                Password = "",
                VirtualHost = "",
                Port = 0
            };

            IEnumerable<string> result = config.GetValidationErrors(0);
            
            Assert.True(result.Count() == 6);
        }
    }
}