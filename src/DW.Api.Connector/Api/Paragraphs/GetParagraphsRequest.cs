using DW.Api.Connector.Models;
using DW.Api.Connector.Services;
using MediatR;
using RestSharp;

namespace DW.Api.Connector.Api.Paragraphs;

public class GetParagraphsRequest : IRequest<GetParagraphsResponse[]>
{
    public int AreaId { get; set; }
    public int PageId { get; set; }
}

public class GetParagraphsResponse : Paragraph
{
}

public class GetParagraphsRequestHandler : IRequestHandler<GetParagraphsRequest, GetParagraphsResponse[]>
{
    private readonly IDWClient _client;

    public GetParagraphsRequestHandler(IDWClient client)
    {
        _client = client;
    }

    public async Task<GetParagraphsResponse[]> Handle(GetParagraphsRequest getParagraphsRequest, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/dwapi/content/paragraphs").AddJsonBody(getParagraphsRequest);

        request.AddParameter("areaId", getParagraphsRequest.AreaId);
        request.AddParameter("pageId", getParagraphsRequest.PageId);

        var paragraphs = await _client.ExecuteAsync<GetParagraphsResponse[]>(request, cancellationToken);
        return paragraphs;
    }
}