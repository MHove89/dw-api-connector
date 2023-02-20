using DW.Api.Connector.Extensions;
using DW.Api.Connector.Models;
using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace DW.Api.Connector.Providers;

public class EnterspeedConnectionProvider : IEnterspeedConnectionProvider
{
    private Dictionary<ConnectionType, IEnterspeedConnection> _connections;
    private readonly IEnterspeedConfigurationService _configurationService;
    private readonly IEnterspeedConfigurationProvider _configurationProvider;

    public EnterspeedConnectionProvider(
        IEnterspeedConfigurationService configurationService,
        IEnterspeedConfigurationProvider configurationProvider)
    {
        _configurationService = configurationService;
        _configurationProvider = configurationProvider;
        Initialize();
    }

    public IEnterspeedConnection GetConnection(ConnectionType type)
    {
        if (!_connections.TryGetValue(type, out var connection))
        {
            return null;
        }

        return connection;
    }

    public void Initialize()
    {
        _connections = new Dictionary<ConnectionType, IEnterspeedConnection>();

        var configuration = _configurationService.GetConfiguration();
        var publishConfiguration = configuration?.GetPublishConfiguration();

        if (publishConfiguration != null)
        {
            _connections.Add(ConnectionType.Publish, new EnterspeedConnection(_configurationProvider));
        }
    }
}