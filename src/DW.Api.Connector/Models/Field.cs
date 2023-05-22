using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Field
{
    [JsonPropertyName("Name")]  
    public string Name { get; set; }

    [JsonPropertyName("SystemName")]
    public string SystemName { get; set; }

    [JsonPropertyName("Value")]
    public object Value { get; set; }
}
