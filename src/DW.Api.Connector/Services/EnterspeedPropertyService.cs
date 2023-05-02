using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using DW.Api.Connector.Extensions;
using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services;

public class EnterspeedPropertyService : IEnterspeedPropertyService
{
    public IEnterspeedProperty CreateMetaData(Page page)
    {
        var metaData = new Dictionary<string, IEnterspeedProperty>
        {
            ["name"] = new StringEnterspeedProperty("name", page.Name),
            ["culture"] = new StringEnterspeedProperty("culture", page.Culture),
            ["createDate"] = new StringEnterspeedProperty("createDate", page.CreatedDate.ToEnterspeedFormatString()),
            ["updateDate"] = new StringEnterspeedProperty("updateDate", page.UpdatedDate.ToEnterspeedFormatString()),
        };

        return new ObjectEnterspeedProperty("metaData", metaData);
    }

    public IEnterspeedProperty CreateMetaData(Paragraph paragraph, string culture)
    {
        var metaData = new Dictionary<string, IEnterspeedProperty>
        {
            ["name"] = new StringEnterspeedProperty("name", paragraph.Name),
            ["culture"] = new StringEnterspeedProperty("culture", culture),
            ["createDate"] = new StringEnterspeedProperty("createDate", paragraph.CreatedDate.ToEnterspeedFormatString()),
            ["updateDate"] = new StringEnterspeedProperty("updateDate", paragraph.UpdatedDate.ToEnterspeedFormatString()),
        };

        return new ObjectEnterspeedProperty("metaData", metaData);
    }

    public IDictionary<string, IEnterspeedProperty> ConvertProperties(Page page)
    {
        var output = new Dictionary<string, IEnterspeedProperty>();
        var metaData = CreateMetaData(page);

        output.Add("metaData", metaData);

        if (page.Item?.Fields != null)
        {
            foreach (var field in page.Item.Fields)
            {
                var jsonElement = JsonSerializer.SerializeToElement(field);
                var properties = jsonElement.EnumerateObject();
                if (!field.SystemName.Equals("Options"))
                {
                    var valueProperty = properties.FirstOrDefault(p => p.Name == "Value");
                    output.Add(field.SystemName, ResolveProperty(valueProperty, field.Name));
                }
            }
        }

        if (page.Paragraphs != null)
        {
            var paragraphItems = new List<IEnterspeedProperty>();

            foreach (var paragraph in page.Paragraphs)
            {
                var paragraphItem = new Dictionary<string, IEnterspeedProperty>();

                var metaProperties = CreateMetaData(paragraph, page.Culture);
                paragraphItem.Add("meta", metaProperties);

                if (paragraph.Item != null)
                {
                    foreach (var field in paragraph.Item.Fields)
                    {
                        var jsonElement = JsonSerializer.SerializeToElement(field);
                        var properties = jsonElement.EnumerateObject();

                        var valueProperty = properties.FirstOrDefault(p => p.Name == "Value");

                        paragraphItem.Add(field.SystemName, ResolveProperty(valueProperty, field.Name));
                    }
                }

                paragraphItems.Add(new ObjectEnterspeedProperty(paragraphItem));
            }

            output.Add("paragraphs", new ArrayEnterspeedProperty("paragraphs", paragraphItems.ToArray()));
        }

        return output;
    }

    public IEnterspeedProperty ResolveProperty(JsonProperty jsonProperty, string properyAlias)
    {

        if (jsonProperty.Name.Equals("Options"))
            return new StringEnterspeedProperty("");

        switch (jsonProperty.Value.ValueKind)
        {
            case JsonValueKind.Undefined:
                break;

            case JsonValueKind.Object:
                var fieldProperties = new Dictionary<string, IEnterspeedProperty>();
                var objectProperties = jsonProperty.Value.EnumerateObject();
                foreach (var objectProperty in objectProperties)
                {
                    fieldProperties.Add(objectProperty.Name, ResolveProperty(objectProperty, properyAlias));
                }

                return new ObjectEnterspeedProperty(fieldProperties);

            case JsonValueKind.Array:
                var objectArray = jsonProperty.Value.EnumerateArray();
                var properties = new List<IEnterspeedProperty>();
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
                                objectFieldProperties.Add(arrayObjectProperty.Name, ResolveProperty(arrayObjectProperty, properyAlias));
                            }

                            properties.Add(new ObjectEnterspeedProperty(objectFieldProperties));
                            break;

                        case JsonValueKind.String:
                            if (jsonProperty.Name == "SelectedNames" && properyAlias == "Tags")
                            {
                                var stringJsonElement = jsonElement.GetString();
                                string pattern = @"\((\d+)\)";
                                var replacement = "";
                                if (stringJsonElement != null)
                                {
                                    var result = Regex.Replace(stringJsonElement, pattern, replacement);
                                    properties.Add(new StringEnterspeedProperty(HttpUtility.HtmlDecode(result?.TrimEnd())));
                                }
                            }
                            else
                            {
                                properties.Add(new StringEnterspeedProperty(HttpUtility.HtmlDecode(jsonElement.GetString())));
                            }
                            break;

                        case JsonValueKind.Number:
                            properties.Add(new NumberEnterspeedProperty(jsonElement.GetDouble()));
                            break;

                        case JsonValueKind.True:
                        case JsonValueKind.False:
                            properties.Add(new BooleanEnterspeedProperty(jsonElement.GetBoolean()));
                            break;

                        case JsonValueKind.Null:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return new ArrayEnterspeedProperty(jsonProperty.Name, properties.ToArray());

            case JsonValueKind.String:
                return new StringEnterspeedProperty(HttpUtility.HtmlDecode(jsonProperty.Value.ToString()));

            case JsonValueKind.Number:
                return new NumberEnterspeedProperty(double.Parse(jsonProperty.Value.ToString()));

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