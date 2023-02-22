using System.Text.Json;
using DW.Api.Connector.Extensions;
using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services;

public class EnterspeedPropertyService : IEnterspeedPropertyService
{
    public IEnterspeedProperty CreateMetaData(Page page, string culture)
    {
        var metaData = new Dictionary<string, IEnterspeedProperty>
        {
            ["name"] = new StringEnterspeedProperty("name", page.Name),
            ["culture"] = new StringEnterspeedProperty("culture", culture),
            ["createDate"] = new StringEnterspeedProperty("createDate", page.CreatedDate.ToEnterspeedFormatString()),
            ["updateDate"] = new StringEnterspeedProperty("updateDate", page.UpdatedDate.ToEnterspeedFormatString()),
        };
        return new ObjectEnterspeedProperty("metaData", metaData);
    }

    public IEnterspeedProperty CreateMetaData(Paragraph paragraph, string culture)
    {
        var metaData = new Dictionary<string, IEnterspeedProperty>
        {
            ["name"] = new StringEnterspeedProperty("name", page.Name),
            ["culture"] = new StringEnterspeedProperty("culture", culture),
            ["createDate"] = new StringEnterspeedProperty("createDate", page.CreatedDate.ToEnterspeedFormatString()),
            ["updateDate"] = new StringEnterspeedProperty("updateDate", page.UpdatedDate.ToEnterspeedFormatString()),
        };
        return new ObjectEnterspeedProperty("metaData", metaData);
    }

    public IDictionary<string, IEnterspeedProperty> ConvertProperties(Page page)
    {
        var output = new Dictionary<string, IEnterspeedProperty>();

        if (page.Item?.Fields != null)
        {
            foreach (var field in page.Item.Fields)
            {
                var fieldEnterspeedProperties = new Dictionary<string, IEnterspeedProperty>();
                var jsonElement = JsonSerializer.SerializeToElement(field);
                var properties = jsonElement.EnumerateObject();

                fieldEnterspeedProperties.Add("name", new StringEnterspeedProperty(field.Name));
                fieldEnterspeedProperties.Add("systemName", new StringEnterspeedProperty(field.SystemName));

                var valueProperty = properties.FirstOrDefault(p => p.Name == "Value");
                fieldEnterspeedProperties.Add("value", ResolveProperty(valueProperty));

                output.Add(field.SystemName, new ObjectEnterspeedProperty(fieldEnterspeedProperties));
            }
        }

        if (page.Paragraphs != null)
        {
            foreach (var paragraph in page.Paragraphs)
            {
                var paragraphEnterspeedProperties = new Dictionary<string, IEnterspeedProperty>();

                var metaProperties = CreateMetaData(paragraph, "");
                paragraphEnterspeedProperties.Add("meta", metaProperties);
            }
        }

        return output;
    }

    public IEnterspeedProperty ResolveProperty(JsonProperty jsonProperty)
    {
        switch (jsonProperty.Value.ValueKind)
        {
            case JsonValueKind.Undefined:
                break;

            case JsonValueKind.Object:
                var fieldEnterspeedProperties = new Dictionary<string, IEnterspeedProperty>();
                var objectProperties = jsonProperty.Value.EnumerateObject();
                foreach (var objectProperty in objectProperties)
                {
                    fieldEnterspeedProperties.Add(objectProperty.Name, ResolveProperty(objectProperty));
                }

                return new ObjectEnterspeedProperty(fieldEnterspeedProperties);

            case JsonValueKind.Array:
                var objectArray = jsonProperty.Value.EnumerateArray();
                var enterspeedProperties = new List<IEnterspeedProperty>();

                foreach (var jsonElement in objectArray)
                {
                    switch (jsonElement.ValueKind)
                    {
                        case JsonValueKind.Undefined:
                            break;

                        case JsonValueKind.Object:
                            var arrayObjectProperties = jsonElement.EnumerateObject();
                            var objectFieldProperties = new Dictionary<string, IEnterspeedProperty>();

                            foreach (var arrayObjectProperty in arrayObjectProperties)
                            {
                                objectFieldProperties.Add(arrayObjectProperty.Name, ResolveProperty(arrayObjectProperty));
                            }

                            enterspeedProperties.Add(new ObjectEnterspeedProperty(objectFieldProperties));
                            break;

                        case JsonValueKind.String:
                            enterspeedProperties.Add(new StringEnterspeedProperty(jsonElement.GetString()));
                            break;

                        case JsonValueKind.Number:
                            enterspeedProperties.Add(new NumberEnterspeedProperty(jsonElement.GetDouble()));
                            break;

                        case JsonValueKind.True:
                        case JsonValueKind.False:
                            enterspeedProperties.Add(new BooleanEnterspeedProperty(jsonElement.GetBoolean()));
                            break;

                        case JsonValueKind.Null:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return new ArrayEnterspeedProperty(jsonProperty.Name, enterspeedProperties.ToArray());

            case JsonValueKind.String:
                return new StringEnterspeedProperty(jsonProperty.Value.ToString());

            case JsonValueKind.Number:
                return new NumberEnterspeedProperty(double.Parse(jsonProperty.Value.ToString() ?? string.Empty));

            case JsonValueKind.True:
            case JsonValueKind.False:
                return new BooleanEnterspeedProperty(jsonProperty.Value.ToString() == "True");

            case JsonValueKind.Null:
                break;

            default:
                return new StringEnterspeedProperty("");
        }

        return new StringEnterspeedProperty("");
    }
}