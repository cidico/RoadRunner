using System;
using RoadRunner.Config.Channels;
using Xunit;

namespace RoadRunner.Tests.Config.Channels
{
    public class ChannelIdentifierTests
    {
        [Fact]
        public void ChannelIdentifier_MustMatchParameter()
        {
            var channel = new ChannelIdentifier("identifier", "key");
            
            Assert.Equal("identifier", channel.ChannelName);
            Assert.Equal("key", channel.ChannelKey);
        }

        [Fact]
        public void EmptyParameters_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new ChannelIdentifier("", ""));
        }

        [Fact]
        public void HashCode_MustMatch()
        {
            var hashCode = $"ChannelName-ChannelKey".GetHashCode();

            var channel = new ChannelIdentifier("ChannelName", "ChannelKey");
            
            Assert.Equal(hashCode, channel.GetHashCode());
        }

        [Fact]
        public void Equals_MustBe_True()
        {
            var chan1 = new ChannelIdentifier("name", "key");
            var chan2 = new ChannelIdentifier("name", "key");
            
            Assert.True(Equals(chan1, chan2));
            Assert.True(chan1.GetHashCode() == chan2.GetHashCode());
        }
    }
}