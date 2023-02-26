using DW.Api.Connector.Models;
using DW.Api.Connector.Services;
using MediatR;
using RestSharp;

namespace DW.Api.Connector.Api.Pages;

public class GetPagesRequest : IRequest<GetPagesResponse[]?>
{
    public int AreaId { get; set; }
    public string? ItemType { get; set; }
}

public class GetPagesResponse : Page
{
}

public class GetPagesRequestHandler : IRequestHandler<GetPagesRequest, GetPagesResponse[]>
{
    private readonly IDWClient _client;

    public GetPagesRequestHandler(IDWClient client)
    {
        _client = client;
    }

    public async Task<GetPagesResponse[]> Handle(GetPagesRequest getPagesRequest, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/dwapi/content/pages")
            .AddJsonBody(getPagesRequest);

        request.Parameters.AddParameter(new QueryParameter("AreaId", getPagesRequest.AreaId.ToString()));
        request.Parameters.AddParameter(new QueryParameter("ItemType", getPagesRequest.ItemType));

        var response = await _client.ExecuteAsync<GetPagesResponse[]>(request, cancellationToken);
        return response;
    }
}