using DW.Api.Connector.Services;

namespace DW.Api.Connector.Configuration;

public class GlobalOptions
{
    public OutputStyle OutPutStyle { get; private set; }

    public void Set(OutputStyle outPutStyle)
    {
        OutPutStyle = outPutStyle;
    }
}