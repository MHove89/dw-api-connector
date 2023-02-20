using System.CommandLine;
using System.CommandLine.Invocation;
using DW.Api.Connector.Api.Pages;
using DW.Api.Connector.Api.Paragraphs;
using DW.Api.Connector.Models.Enterspeed;
using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Services;
using MediatR;

namespace DW.Api.Connector.Commands.Pages;

public class IngestPagesCommmand : Command
{
    public IngestPagesCommmand() : base(name: "ingestPages", "Ingest pages by parentId (optional : parentPageId)")
    {
        AddArgument(new Argument<int>("areaId", "Id of parent page") { Arity = ArgumentArity.ExactlyOne });
        AddArgument(new Argument<string>("itemType", "Id of parent page") { Arity = ArgumentArity.ExactlyOne });
    }

    public new class Handler : BaseCommandHandler, ICommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IOutputService _outputService;
        private readonly IEnterspeedIngestService _enterspeedIngestService;
        public int AreaId { get; set; }
        public string? ItemType { get; set; }


        public Handler(IMediator mediator,
            IOutputService outputService,
            IEnterspeedIngestService enterspeedIngestService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _outputService = outputService;
            _enterspeedIngestService = enterspeedIngestService;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var pages = await GetPages();
            await AddParagraphsToPages(pages);
            
            foreach (var page in pages)
            {
                _enterspeedIngestService.Save(new DWContentEntity(page, "fdsa"));
            }
            
            return 0;
        }

        private async Task<GetPagesResponse[]> GetPages()
        {
            var pages = await _mediator.Send(new GetPagesRequest()
            {
                AreaId = AreaId,
                ItemType = ItemType
            });
            return pages;
        }

        private async Task AddParagraphsToPages(GetPagesResponse[] pages)
        {
            foreach (var page in pages)
            {
                var paragraphs = await _mediator.Send(new GetParagraphsRequest()
                {
                    AreaId = page.AreaID,
                    PageId = page.ID
                });

                page.Paragraphs.AddRange(paragraphs);
            }
        }
    }
}