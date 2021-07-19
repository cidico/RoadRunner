
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using RoadRunner.Config;
using Xunit;

namespace RoadRunner.Tests.Config
{
    public class ConfigLoaderTests : TestBase
    {
        private const string SectionName = "RoadRunner";
        
        [Fact]
        public void NullConfiguration_ShouldThrow()
        {
            IConfiguration config = null;
            
            Assert.Throws<FileNotFoundException>(()=> ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void ConfigurationWithoutRoadRunner_AndNotProvidingConfigFile_ShouldThrow()
        {
            var config = this.BuildConfigurationWithoutRoadRunnerSection();
            
            Assert.Throws<FileNotFoundException>(()=> ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void Configuration_InvalidPublisherName_ShouldThrow()
        {
            var settings = this.CreateRootSection();
            settings.AddRange(this.CreateBasicPublisher(
                publisherIndex: 0,
                enabled: true,
                publisherName: "",
                server: "test",
                port: 1000, 
                username: "guest", 
                password: "guest",
                virutalHost: "/"
            ));

            var config = this.BuildSettings(settings);

            Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void Configuration_InvalidServer_ShouldThrow()
        {
            var settings = this.CreateRootSection();
            settings.AddRange(this.CreateBasicPublisher(
                publisherIndex: 0,
                enabled: true,
                publisherName: "test",
                server: "",
                port: 5672,
                username: "guest", 
                password: "guest",
                virutalHost: "/"
            ));

            var config = this.BuildSettings(settings);

            Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void Configuration_InvalidPort_ShouldThrow()
        {
            var settings = this.CreateRootSection();
            settings.AddRange(this.CreateBasicPublisher(
                publisherIndex: 0,
                enabled: true,
                publisherName: "test",
                server: "localhost",
                port: 0,
                username: "guest", 
                password: "guest",
                virutalHost: "/"
            ));

            var config = this.BuildSettings(settings);

            Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void Configuration_InvalidUsername_ShouldThrow()
        {
            var settings = this.CreateRootSection();
            settings.AddRange(this.CreateBasicPublisher(
                publisherIndex: 0,
                enabled: true,
                publisherName: "test",
                server: "localhost",
                port: 5672,
                username: "", 
                password: "guest",
                virutalHost: "/"
            ));

            var config = this.BuildSettings(settings);

            Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadConfigs(config));
        }
        
        [Fact]
        public void Configuration_InvalidPassword_ShouldThrow()
        {
            var settings = this.CreateRootSection();
            settings.AddRange(this.CreateBasicPublisher(
                publisherIndex: 0,
                enabled: true,
                publisherName: "test",
                server: "localhost",
                port: 5672,
                username: "guest", 
                password: "",
                virutalHost: "/"
            ));

            var config = this.BuildSettings(settings);

            Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadConfigs(config));
        }
    }
}