using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using DW.Api.Connector.Api.Pages;
using DW.Api.Connector.Api.Paragraphs;
using DW.Api.Connector.Models;
using DW.Api.Connector.Models.Enterspeed;
using DW.Api.Connector.Services;
using Enterspeed.Source.Sdk.Api.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DW.Api.Connector.Commands.Pages;

public class IngestPagesCommmand : Command
{
    public IngestPagesCommmand() : base(name: "ingestPages", "Ingest pages by parentId (optional : parentPageId)")
    {
        AddArgument(new Argument<int>("areaId", "Id of parent page") { Arity = ArgumentArity.ExactlyOne });
        AddArgument(new Argument<string>("itemType", "Id of parent page") { Arity = ArgumentArity.ExactlyOne });
        AddArgument(new Argument<string>("culture", "Language Culture") { Arity = ArgumentArity.ExactlyOne });
    }

    public new class Handler : BaseCommandHandler, ICommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IOutputService _outputService;
        private readonly IEnterspeedIngestService _enterspeedIngestService;
        private readonly IEnterspeedPropertyService _enterspeedPropertyService;
        private readonly ILogger<Handler> _logger;
        public int AreaId { get; set; }
        public string? ItemType { get; set; }
        public string Culture { get; set; }

        public Handler(IMediator mediator,
            IOutputService outputService,
            IEnterspeedIngestService enterspeedIngestService,
            ILogger<Handler> logger,
            IEnterspeedPropertyService enterspeedPropertyService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _outputService = outputService;
            _enterspeedIngestService = enterspeedIngestService;
            _logger = logger;
            _enterspeedPropertyService = enterspeedPropertyService;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var pages = await GetPages();
            var castedPages = pages.Cast<Page>().ToList();
            await AddParagraphsToPages(castedPages);

            foreach (var page in castedPages)
            {
                page.Culture = Culture;
                var dwEntity = new DWContentEnterspeedEntity(page, _enterspeedPropertyService);
                var response = _enterspeedIngestService.Save(dwEntity);
                if (!response.Success)
                {
                    var message = JsonSerializer.Serialize(new ErrorResponse(response));
                    throw new Exception($"Failed ingesting entity ({dwEntity.Id}). Message: {message}");
                }
            }

            return 0;
        }

        private async Task<GetPagesResponse[]> GetPages()
        {
            var itemTypes = new List<string>();
            var response = new List<GetPagesResponse>();

            if (ItemType != null && ItemType.Contains(','))
            {
                var itemTypesSplitted = ItemType.Replace(" ", "").Split(',');
                foreach (var itemType in itemTypesSplitted)
                {
                    itemTypes.Add(itemType);
                }
            }
            else if (!string.IsNullOrEmpty(ItemType))
            {
                itemTypes.Add(ItemType);
            }

            foreach (var itemType in itemTypes)
            {
                var pages = await _mediator.Send(new GetPagesRequest()
                {
                    AreaId = AreaId,
                    ItemType = itemType
                });

                if (pages != null)
                {
                    response.AddRange(pages);
                }
            }

            return response.ToArray();
        }

        private async Task AddParagraphsToPages(List<Page> pages)
        {
            foreach (var page in pages)
            {
                var paragraphs = await _mediator.Send(new GetParagraphsRequest()
                {
                    AreaId = page.AreaID,
                    PageId = page.ID
                });

                var paragraphList = new List<Paragraph>();
                foreach (var paragraphResponse in paragraphs)
                {
                    var paragraph = paragraphResponse as Paragraph;
                    paragraphList.Add(paragraphResponse);
                }

                page.Paragraphs = paragraphList;
            }
        }
    }
}