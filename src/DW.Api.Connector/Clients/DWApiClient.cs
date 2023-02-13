public class DWApiClient : IDWApiClient
{
    private readonly HttpClient _client;
    public DWApiClient(HttpClient client)
    {
        _client = client;
    }
}