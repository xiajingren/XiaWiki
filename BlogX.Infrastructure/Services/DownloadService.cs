using System.Net.Http;
using BlogX.Core.Interfaces;

namespace BlogX.Infrastructure.Services;

public class DownloadService : IDownloadService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DownloadService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Stream> DownloadAsync(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
}
