using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Language
{
    [JsonPropertyName("ID")]
    public int ID { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Culture")]
    public string? Culture { get; set; }

    [JsonPropertyName("PrimaryDomain")]
    public string? PrimaryDomain { get; set; }

    [JsonPropertyName("IsCurrent")]
    public bool IsCurrent { get; set; }

    [JsonPropertyName("IsMaster")]
    public bool IsMaster { get; set; }

    [JsonPropertyName("PageIsHidden")]
    public bool PageIsHidden { get; set; }

    [JsonPropertyName("PageIsPublished")]
    public bool PageIsPublished { get; set; }
}