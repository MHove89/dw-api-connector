using DW.Api.Connector.Configuration;

namespace DW.Api.Connector.Services;

public interface IEnterspeedConfigurationService
{
    void Save(EnterspeedDWConfiguration configuration);
    EnterspeedDWConfiguration GetConfiguration();
    bool IsPublishConfigured();
    bool IsPreviewConfigured();
}