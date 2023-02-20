using DW.Api.Connector.Configuration;

namespace DW.Api.Connector.Extensions;

public static class EnterspeedConfigurationExtensions
{
    public static EnterspeedDWConfiguration GetPublishConfiguration(this EnterspeedDWConfiguration me)
    {
        if (me == null)
        {
            return null;
        }

        return new EnterspeedDWConfiguration
        {
            ApiKey = me.ApiKey,
            BaseUrl = me.BaseUrl,
            ConnectionTimeout = me.ConnectionTimeout,
            MediaDomain = me.MediaDomain
        };
    }
}