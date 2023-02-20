using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Models.Enterspeed;

public class DWContentEntity : IEnterspeedEntity
{
    private readonly Page _page;

    public DWContentEntity(Page page, string pageType)
    {
        _page = page;
        Type = pageType;
    }

    public string Id => _page.ID.ToString();
    public string Type { get; }
    public string Url => !string.IsNullOrEmpty(_page.Link) ? _page.Link : "";
    public string[] Redirects { get; }
    public string ParentId => (_page?.Path?[_page.Path.Count - 1] != null ? _page.Path[_page.Path.Count - 1]?.ID.ToString() : "") ?? string.Empty;
    public IDictionary<string, IEnterspeedProperty> Properties { get; }
}