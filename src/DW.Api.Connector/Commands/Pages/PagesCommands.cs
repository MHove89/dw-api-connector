using System.CommandLine;

namespace DW.Api.Connector.Commands.Pages;

public static class PagesCommands
{
    public static Command BuildCommands()
    {
        var command = new Command("pages", "Pages")
        {
            new IngestPagesCommmand(),
        };

        return command;
    }
}