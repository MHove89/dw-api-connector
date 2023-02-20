using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Models.Enterspeed;

public class DWContentEntity : IEnterspeedEntity
{
    private readonly Page _page;

    public DWContentEntity(Page page, string pageType)
    {
        _page = page;
    }

    public string Id => _page.ID.ToString();
    public string Type => _page.Item.SystemName;
    public string Url => !string.IsNullOrEmpty(_page.Item.Link) ? _page.Item.Link : "";
    public string[] Redirects { get; }
    public string ParentId => (_page?.Path?[^2] != null ? _page.Path[^2]?.ID.ToString() : "") ?? string.Empty;
    public IDictionary<string, IEnterspeedProperty> Properties { get; }
}