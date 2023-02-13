using DW.Api.Connector.Models;
using DW.Api.Connector.Services;
using MediatR;
using RestSharp;

namespace DW.Api.Connector.Api.Pages;

public class GetPagesRequest : IRequest<GetPagesResponse>
{
}

public class GetPagesResponse
{
    public IEnumerable<Page>? Pages { get; set; }
}

public class GetEnvironmentsRequestHandler : IRequestHandler<GetPagesRequest, GetPagesResponse>
{
    private readonly IDWClient _client;

    public GetEnvironmentsRequestHandler(IDWClient client)
    {
        _client = client;
    }

    public async Task<GetPagesResponse> Handle(GetPagesRequest getPagesRequest, CancellationToken cancellationToken)
    {
        var request = new RestRequest("tenant/environments")
            .AddJsonBody(getPagesRequest);

        var response = await _client.ExecuteAsync<GetPagesResponse>(request, cancellationToken);
        return response;
    }
}