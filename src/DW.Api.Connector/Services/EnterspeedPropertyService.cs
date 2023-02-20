using DW.Api.Connector.Extensions;
using DW.Api.Connector.Models;
using DW.Api.Connector.Models.Enterspeed;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services;

public class EnterspeedPropertyService : IEnterspeedPropertyService
{
    public IEnterspeedProperty CreateNodeMetaData(Page page, string culture)
    {
        var metaData = new Dictionary<string, IEnterspeedProperty>
        {
            ["name"] = new StringEnterspeedProperty("name", page.Name),
            ["culture"] = new StringEnterspeedProperty("culture", culture),
            // ["domain"] = new StringEnterspeedProperty("domain", GetDomain(content, culture)?.DomainName),
            // ["sortOrder"] = new NumberEnterspeedProperty("sortOrder", content.SortOrder),
            // ["level"] = new NumberEnterspeedProperty("level", content.Level),
            ["createDate"] = new StringEnterspeedProperty("createDate", page.CreatedDate.ToEnterspeedFormatString()),
            ["updateDate"] = new StringEnterspeedProperty("updateDate", page.UpdatedDate.ToEnterspeedFormatString()),
            // ["nodePath"] = new ArrayEnterspeedProperty("nodePath", GetNodePath(content.Path, culture))
        };
        return new ObjectEnterspeedProperty("metaData", metaData);
    }

    public IDictionary<string, IEnterspeedProperty> ConvertProperties(List<Field>? fields)
    {
        var output = new Dictionary<string, IEnterspeedProperty>();

        if (fields != null)
        {
            foreach (var field in fields)
            {
                // Figure out a way to resolve types before sending them to enterspeed. 
            }
        }

        return output;
    }
}