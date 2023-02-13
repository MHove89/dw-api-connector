using DW.Api.Connector.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DW.Api.Connector;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);

                var dwApiConnectorSettings = GetAppSettings(services);
                services.AddSingleton(dwApiConnectorSettings);

                if (!string.IsNullOrEmpty(dwApiConnectorSettings.BaseAddress))
                {
                    services.AddHttpClient<IDWApiClient, DWApiClient>((client) => { client.BaseAddress = new Uri(dwApiConnectorSettings.BaseAddress); });
                }

                services.AddHostedService<Startup>();
            });

        host.Start();
    }

    private static DWAPIConnectorSettings GetAppSettings(IServiceCollection services)
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