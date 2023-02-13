using System.CommandLine;
using System.CommandLine.Invocation;
using DW.Api.Connector.Configuration;
using DW.Api.Connector.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DW.Api.Connector.Extensions;

public class MiddleWare
{
    public static Action<InvocationContext> ApiKeyAuth(Option<string> apiKeyOption, Option<OutputStyle> outPutStyle)
    {
        return (context) =>
        {
            var outPutStyleValue = context.ParseResult.GetValueForOption(outPutStyle);

            var host = context.BindingContext.GetService<IHost>();
            var globalOptions = host?.Services.GetService<GlobalOptions>();

            globalOptions?.Set(outPutStyleValue);
        };
    }
}