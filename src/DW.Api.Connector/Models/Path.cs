using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Path
{
    [JsonPropertyName("ID")]
    public int ID { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
}