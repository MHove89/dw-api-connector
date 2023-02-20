using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services;

public interface IEnterspeedPropertyService
{
    IEnterspeedProperty CreateNodeMetaData(Page page, string culture);
    IDictionary<string, IEnterspeedProperty> ConvertProperties(List<Field>? fields);
}