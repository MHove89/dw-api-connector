using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Connection;

namespace DW.Api.Connector.Providers;

public interface IEnterspeedConnectionProvider
{
    IEnterspeedConnection GetConnection(ConnectionType type);
    void Initialize();
}