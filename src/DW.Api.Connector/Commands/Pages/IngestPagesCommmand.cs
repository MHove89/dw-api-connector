using System.CommandLine;
using System.CommandLine.Invocation;
using DW.Api.Connector.Api.Pages;
using DW.Api.Connector.Services;
using MediatR;

namespace DW.Api.Connector.Commands.Pages;

public class IngestPagesCommmand : Command
{
    public IngestPagesCommmand() : base(name: "ingestPages", "Ingest pages by parentId (optional : pageId)")
    {
    }

    public new class Handler : BaseCommandHandler, ICommandHandler
    {
        private readonly IMediator _mediator;
        private readonly IOutputService _outputService;

        public Handler(IMediator mediator, IOutputService outputService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _outputService = outputService;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var response = await _mediator.Send(new GetPagesRequest());

            _outputService.Write(response);
            return 0;
        }
    }
}