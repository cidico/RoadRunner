using System;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;

namespace RoadRunner.Log
{
    public static class RoadRunnerLogger 
    { 
        private static Lazy<LogFactory> instance = new Lazy<LogFactory>(BuildLogFactory);
        
        // A Logger dispenser for the current assembly (Remember to call Flush on application exit)
        public static LogFactory Instance => instance.Value;
 
        private static LogFactory BuildLogFactory()
        {
            // Use name of current assembly to construct NLog config filename 
            Assembly thisAssembly = Assembly.GetExecutingAssembly(); 
            string configFilePath = Path.Combine( Path.GetDirectoryName(thisAssembly.Location)!, "NLog.config"); 

            var logFactory = new LogFactory();
            logFactory.Configuration = new XmlLoggingConfiguration(configFilePath, logFactory); 
            
            return logFactory;
        }
    }
}