using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Configuration;

namespace DW.Api.Connector.Providers;

public class EnterspeedDWConfigurationProvider : IEnterspeedConfigurationProvider
{
    private readonly IEnterspeedConfigurationService _enterspeedConfigurationService;

    public EnterspeedDWConfigurationProvider(IEnterspeedConfigurationService enterspeedConfigurationService)
    {
        _enterspeedConfigurationService = enterspeedConfigurationService;
    }

    public EnterspeedConfiguration Configuration => _enterspeedConfigurationService.GetConfiguration();
}