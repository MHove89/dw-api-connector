namespace DW.Api.Connector.Services;

public interface IOutputService
{
    void Write<T>(T value);
}