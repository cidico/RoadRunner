using System.Collections.Generic;
using System.Linq;
using RoadRunner.Config.Channels;
using Xunit;

namespace RoadRunner.Tests.Config.Channels
{
    public class ChannelConfigurationTests : TestBase
    {
        [Fact]
        public void Configuration_WithValidData_ShouldNotAddError()
        {
            var config = new ChannelConfiguration
            {
                Enabled = true,
                Name = "channel",
                ExchangeName = "exchange",
                RoutingKey = "/"
            };

            IEnumerable<string> result = config.GetValidationErrors(0);
            
            Assert.False(result.Any());
        }
        
        [Fact]
        public void Configuration_WithInvalidData_ShouldAddError()
        {
            var config = new ChannelConfiguration
            {
                Enabled = true,
                Name = "",
                ExchangeName = "",
                RoutingKey = ""
            };

            IEnumerable<string> result = config.GetValidationErrors(0).ToArray();
            
            Assert.True(result.Any());
            Assert.True(result.Count() == 3);
        }
    }
}