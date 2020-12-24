using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

// Console app which implements: Dependency Injection, Serilog, and AppSettings.

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Establish configuration early to enable logging (pass in the builder from the custom BuildConfig method).
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            // Setup Serilog logging.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            // Initialize the host along with its configurations, DI and logging.
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            // Create an instance of the host's greeting service and run it.
            var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            svc.Run();
        }

        // Create a manual connection to configuration source 
        static void BuildConfig(IConfigurationBuilder builder)
        {
            // Retrieve standard and environment-specific appsettings and any environment variables.
            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
        }

    }
}
