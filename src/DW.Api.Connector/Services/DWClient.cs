using System.Text.Json;
using DW.Api.Connector.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace DW.Api.Connector.Services;

public class DWClient : IDWClient, IDisposable
{
    private readonly ILogger<DWClient> _logger;
    private readonly RestClient _client;

    public DWClient(ILogger<DWClient> logger,
        GlobalOptions globalOptions,
        DWAPIConnectorSettings settings)
    {
        _logger = logger;

        var options = new RestClientOptions(settings.BaseAddress);
        _client = new RestClient(options);
    }

    public async Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _client.ExecuteAsync<T>(request, cancellationToken);

        if (!response.IsSuccessful)
        {
            _logger.LogError($"Unsuccessful: {response.StatusCode}");
            _logger.LogWarning(JsonSerializer.Serialize(response.Data));
        }

        return response.Data;
    }

    public async Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _client.ExecuteAsync(request, cancellationToken);
        return response;
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }
}
