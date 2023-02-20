using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using DW.Api.Connector.Commands.Pages;
using DW.Api.Connector.Extensions;
using DW.Api.Connector.Providers;
using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Domain.Connection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DW.Api.Connector;

class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);
    private static readonly Option<bool> VerboseLogging = new(new[] { "--verbose", "-v" }, "verbose");

    public static async Task<int> Main(string[] args)
    {
        var runner = BuildCommandLine()
            .UseHost(_ => CreateHostBuilder(args), (builder) => builder
                .ConfigureAppConfiguration((context, configuration) => { configuration.AddJsonFile($"appsettings.local.json", optional: true); })
                .ConfigureServices((_, services) =>
                {
                    services.AddApplication();
                })
                .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ConfigureSerilog(context, VerboseLogging))
                .UseCommands())
            .UseDefaults()
            .Build();

        return await runner.InvokeAsync(args);
    }

    private static CommandLineBuilder BuildCommandLine()
    {
        var root = new RootCommand();
        root.AddCommand(PagesCommands.BuildCommands());
        root.AddGlobalOption(VerboseLogging);

        return new CommandLineBuilder(root);
    }
}