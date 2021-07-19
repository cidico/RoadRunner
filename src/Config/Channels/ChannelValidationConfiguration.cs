namespace RoadRunner.Config.Channels
{
    public class ChannelValidationConfiguration
    {
        public ChannelValidationConfiguration()
        {
            
        }
        
        /// <summary>
        /// Indicates if the publisher needs to verify the message size before send it to the server.
        /// </summary>
        public bool CheckMessageSize { get; set; }

        /// <summary>
        /// Sets the maximum size (in bytes) for a message.
        /// </summary>
        public int MaxMessageSize { get; set; } = 20480;
    }
}