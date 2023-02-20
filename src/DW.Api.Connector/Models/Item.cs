using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Item
{
    [JsonPropertyName("Fields")]
    public List<Field> Fields { get; set; }

    [JsonPropertyName("Id")]
    public string Id { get; set; }

    [JsonPropertyName("SystemName")]
    public string SystemName { get; set; }

    [JsonPropertyName("PageID")]
    public int PageID { get; set; }

    [JsonPropertyName("Link")]
    public string Link { get; set; }
}
