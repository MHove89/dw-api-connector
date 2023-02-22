using System.Text.Json.Serialization;

namespace DW.Api.Connector.Models;

public class Paragraph
{
    [JsonPropertyName("ID")]
    public int ID { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("UpdatedDate")]
    public DateTime UpdatedDate { get; set; }

    [JsonPropertyName("PageID")]
    public int PageID { get; set; }

    [JsonPropertyName("GlobalID")]
    public int GlobalID { get; set; }

    [JsonPropertyName("Text")]
    public string? Text { get; set; }

    [JsonPropertyName("Image")]
    public string? Image { get; set; }

    [JsonPropertyName("ImageFocalX")]
    public int ImageFocalX { get; set; }

    [JsonPropertyName("ImageFocalY")]
    public int ImageFocalY { get; set; }

    [JsonPropertyName("ImageHAlign")]
    public string? ImageHAlign { get; set; }

    [JsonPropertyName("ImageVAlign")]
    public string? ImageVAlign { get; set; }

    [JsonPropertyName("ImageLink")]
    public string? ImageLink { get; set; }

    [JsonPropertyName("ImageAlt")]
    public string? ImageAlt { get; set; }

    [JsonPropertyName("ImageLinkTarget")]
    public string? ImageLinkTarget { get; set; }

    [JsonPropertyName("ImageCaption")]
    public string? ImageCaption { get; set; }

    [JsonPropertyName("Item")]
    public Item? Item { get; set; }
}