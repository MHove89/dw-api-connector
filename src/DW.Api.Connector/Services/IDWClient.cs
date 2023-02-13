using RestSharp;

namespace DW.Api.Connector.Services;

public interface IDWClient
{
    Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default);
    Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default);
}