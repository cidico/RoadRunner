using RoadRunner.Config.Channels;
using Xunit;

namespace RoadRunner.Tests.Config.Channels
{
    public class ChannelValidationConfigurationTests
    {
        [Fact]
        public void Values_MustMatch()
        {
            var cvc = new ChannelValidationConfiguration();
            cvc.CheckMessageSize = true;
            cvc.MaxMessageSize = 10;
        }
    }
}