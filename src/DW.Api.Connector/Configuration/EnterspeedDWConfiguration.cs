using Enterspeed.Source.Sdk.Configuration;

namespace DW.Api.Connector.Configuration;

public class EnterspeedDWConfiguration : EnterspeedConfiguration
{
    public string MediaDomain { get; set; }
    public bool IsConfigured { get; set; }
    public string PreviewApiKey { get; set; }
}