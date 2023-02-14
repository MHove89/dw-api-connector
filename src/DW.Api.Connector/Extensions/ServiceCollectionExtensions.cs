using System.CommandLine;
using System.CommandLine.Hosting;
using System.Reflection;
using DW.Api.Connector.Configuration;
using DW.Api.Connector.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace DW.Api.Connector.Extensions;

public static class ServiceCollectionExtensions
{
    public static LoggerConfiguration ConfigureSerilog(this LoggerConfiguration loggerConfiguration, HostBuilderContext context, Option<bool> verboseLogging)
    {
        var verbose = context.GetInvocationContext().ParseResult.GetValueForOption(verboseLogging);
        var logEventLevel = verbose ? LogEventLevel.Verbose : LogEventLevel.Warning;

        loggerConfiguration.MinimumLevel.Override("Microsoft", logEventLevel);

        var userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var logFilePath = Path.Combine(userFolderPath, ".enterspeed", "cli.log.json");

        loggerConfiguration.WriteTo.File(new CompactJsonFormatter(), logFilePath, logEventLevel);
        loggerConfiguration.WriteTo.Console(logEventLevel);
        loggerConfiguration.Enrich.WithExceptionDetails();

        return loggerConfiguration;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddSingleton(GetSettings(services));
        services.AddTransient<IDWClient, DWClient>();
        services.AddSingleton<GlobalOptions>();
        services.AddTransient<IOutputService, OutputService>();

        return services;
    }

    private static DWAPIConnectorSettings GetSettings(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var dwApiConnectorSettings = new DWAPIConnectorSettings();
        config.Bind("DWAPIConnector", dwApiConnectorSettings);
        return dwApiConnectorSettings;
    }
}