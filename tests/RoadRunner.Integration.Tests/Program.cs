// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using RoadRunner;

Console.WriteLine("Hello, World!");

var config = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json", optional:false, reloadOnChange: true)
    .Build();

Publisher.Initialize(config);

// Telecom
Publisher.Publish("PoiReportV3", "Telecom", "{...}");

// Hardware
Publisher.Publish("PoiReportV3", "Hardware", "{...}");