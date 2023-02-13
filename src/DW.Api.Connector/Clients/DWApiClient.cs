using System.Net.Http.Json;
using DW.Api.Connector.Models.API;

namespace DW.Api.Connector.Clients;

public class DWApiClient : IDWApiClient
{
    private readonly HttpClient _client;

    public DWApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<PagesResponse?> GetPages(int? parentPageId)
    {
        var requestUri = "/dwapi/content/pages";
        if (parentPageId.HasValue)
        {
            requestUri = requestUri + "?pageid=" + parentPageId;
        }

        return await _client.GetFromJsonAsync<PagesResponse>(requestUri);
    }
}