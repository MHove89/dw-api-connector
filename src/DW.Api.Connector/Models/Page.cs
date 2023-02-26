using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Page
{
    [JsonPropertyName("ID")]
    public int ID { get; set; }

    [JsonPropertyName("culture")]
    public string Culture { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("UpdatedDate")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Keywords")]
    public string? Keywords { get; set; }

    [JsonPropertyName("AreaID")]
    public int AreaID { get; set; }

    [JsonPropertyName("Path")]
    public List<Path>? Path { get; set; }

    [JsonPropertyName("Languages")]
    public List<Language>? Languages { get; set; }

    [JsonPropertyName("Item")]
    public Item Item { get; set; }

    [JsonPropertyName("paragraphs")]
    public List<Paragraph>? Paragraphs { get; set; }
}