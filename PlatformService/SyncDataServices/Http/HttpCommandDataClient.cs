using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var body = JsonSerializer.Serialize(platform);
        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");

        var requestUrl = _configuration["CommandService"];
        var response = await _httpClient.PostAsync(requestUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }
}