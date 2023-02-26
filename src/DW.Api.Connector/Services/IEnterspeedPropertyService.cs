using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services;

public interface IEnterspeedPropertyService
{
    IEnterspeedProperty CreateMetaData(Page page);
    IDictionary<string, IEnterspeedProperty> ConvertProperties(Page page);
}