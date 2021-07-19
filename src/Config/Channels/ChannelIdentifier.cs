using System;

namespace RoadRunner.Config.Channels
{
    public class ChannelIdentifier
    {
        public string ChannelName { get; }

        public string ChannelKey { get; }

        public ChannelIdentifier(string name, string key)
        {
            if(name.IsNullOrEmpty()) throw new ArgumentNullException(nameof(name));
            if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
            
            this.ChannelName = name;
            this.ChannelKey = key;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is ChannelIdentifier other)) return false;

            return this.ChannelName == other.ChannelName && this.ChannelKey == other.ChannelKey;
        }
        
        public override int GetHashCode()
        {
            return $"{this.ChannelName}-{this.ChannelKey}".GetHashCode();
        }
    }
}