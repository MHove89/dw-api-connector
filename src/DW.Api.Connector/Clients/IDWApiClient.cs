using DW.Api.Connector.Models.API;

namespace DW.Api.Connector.Clients;

public interface IDWApiClient
{
    Task<PagesResponse?> GetPages(int? parentPageId);
}