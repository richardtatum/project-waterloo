using System.Web;
using InfoTrack.ProjectWaterloo.Scraping.Extensions;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using Microsoft.Extensions.Logging;

namespace InfoTrack.ProjectWaterloo.Scraping.Clients;

public class GoogleClient : ISearchEngineClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GoogleClient> _logger;
    
    public GoogleClient(HttpClient httpClient, ILogger<GoogleClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> SearchAsync(string searchTerm, int results = 100)
    {
        // Maybe don't need matching domain
        // Need to URL encode search term?
        var url = "search"
            .AddQueryParameter("num", results)
            .AddQueryParameter("q", searchTerm);

        var response = await _httpClient.GetAsync(url);
        
        // Need to try catch instead? Or move try catch to scraper
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync()
;    }
}