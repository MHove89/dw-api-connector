using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DW.Api.Connector;

public class Startup : BackgroundService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHost _host;
    private readonly DWAPIConnectorSettings _dwApiConnectorSettings;

    public Startup(
        ILogger<Startup> logger,
        IHost host,
        DWAPIConnectorSettings dwAPIConnectorSettings)
    {
        _logger = logger;
        _host = host;
        _dwApiConnectorSettings = dwAPIConnectorSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.ReadLine();
        await _host.StopAsync();
    }
}