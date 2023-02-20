using DW.Api.Connector.Configuration;
using DW.Api.Connector.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DW.Api.Connector.Services;

public class EnterspeedConfigurationService : IEnterspeedConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    private EnterspeedDWConfiguration _enterspeedUmbracoConfiguration;

    [Obsolete("Use separate configuration keys instead.", false)]
    private readonly string _configurationDatabaseKey = "Enterspeed+Configuration";
    private readonly string _configurationMediaDomainDatabaseKey = "Enterspeed+Configuration+MediaDomain";
    private readonly string _configurationApiKeyDatabaseKey = "Enterspeed+Configuration+ApiKey";
    private readonly string _configurationPreviewApiKeyDatabaseKey = "Enterspeed+Configuration+PreviewApiKey";

    private readonly string _configurationConnectionTimeoutDatabaseKey =
        "Enterspeed+Configuration+ConnectionTimeout";

    private readonly string _configurationBaseUrlDatabaseKey = "Enterspeed+Configuration+BaseUrl";

    public EnterspeedConfigurationService(
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public EnterspeedDWConfiguration GetConfiguration()
    {
        if (_enterspeedUmbracoConfiguration != null)
        {
            return _enterspeedUmbracoConfiguration;
        }

        _enterspeedUmbracoConfiguration = GetConfigurationFromSettingsFile();
        return _enterspeedUmbracoConfiguration;
    }

    public bool IsPublishConfigured()
    {
        var configuration = GetConfiguration();
        return configuration != null && configuration.IsConfigured;
    }

    public bool IsPreviewConfigured()
    {
        var configuration = GetConfiguration();
        return configuration != null
               && configuration.IsConfigured
               && !string.IsNullOrWhiteSpace(configuration.PreviewApiKey);
    }

    public void Save(EnterspeedDWConfiguration configuration)
    {
        if (configuration == null)
        {
            return;
        }

        configuration.MediaDomain = configuration.MediaDomain.TrimEnd('/');

        // Since old configuration single key is Obsolete and will be deprecated, transform it into newest version configuration, and cleanup obsolete version.
        configuration.IsConfigured = true;
        _enterspeedUmbracoConfiguration = configuration;

        // Reinitialize connections in case of changes in the configuration
        var connectionProvider = _serviceProvider.GetRequiredService<IEnterspeedConnectionProvider>();
        connectionProvider.Initialize();
    }

    private EnterspeedDWConfiguration GetConfigurationFromSettingsFile()
    {
        var webConfigEndpoint = _configuration["Enterspeed:Endpoint"];
        var webConfigMediaDomain = _configuration["Enterspeed:MediaDomain"];
        var webConfigApikey = _configuration["Enterspeed:Apikey"];
        var webConfigPreviewApikey = _configuration["Enterspeed:PreviewApikey"];

        if (string.IsNullOrWhiteSpace(webConfigEndpoint) || string.IsNullOrWhiteSpace(webConfigApikey))
        {
            return new EnterspeedDWConfiguration();
        }

        _enterspeedUmbracoConfiguration = new EnterspeedDWConfiguration()
        {
            BaseUrl = webConfigEndpoint?.Trim(),
            ApiKey = webConfigApikey?.Trim(),
            MediaDomain = webConfigMediaDomain?.Trim(),
            IsConfigured = true,
            PreviewApiKey = webConfigPreviewApikey?.Trim()
        };

        return _enterspeedUmbracoConfiguration;
    }
}