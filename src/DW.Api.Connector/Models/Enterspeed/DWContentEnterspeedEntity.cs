using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Models.Enterspeed;

public class DWContentEnterspeedEntity : IEnterspeedEntity
{
    private readonly Page _page;
    private readonly string _websiteDomain;

    public DWContentEnterspeedEntity(
        Page page,
        IEnterspeedPropertyService propertyService,
        string websiteDomain)
    {
        _page = page;
        _websiteDomain = websiteDomain;
        Properties = propertyService.ConvertProperties(page);
    }

    public string Id => _page.ID + "-" + _page.Culture;
    public string Type => _page.Item.SystemName;
    public string Url => !string.IsNullOrEmpty(_page.Item.Link) ? _websiteDomain + _page.Item.Link : "";
    public string[] Redirects { get; }
    public string ParentId => (_page?.Path?[^2] != null ? _page.Path[^2]?.ID.ToString() : "") ?? string.Empty;
    public IDictionary<string, IEnterspeedProperty> Properties { get; }
}