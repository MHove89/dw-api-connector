using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Startup : BackgroundService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHost _host;

    public Startup(ILogger<Startup> logger, IHost host)
    {
        _logger = logger;
        _host = host;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.ReadLine();
        await _host.StopAsync();
    }
}