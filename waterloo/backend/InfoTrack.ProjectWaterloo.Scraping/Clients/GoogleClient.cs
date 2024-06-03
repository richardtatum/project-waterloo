using InfoTrack.ProjectWaterloo.Scraping.Extensions;

namespace InfoTrack.ProjectWaterloo.Scraping.Clients;

public class GoogleClient(HttpClient httpClient)
{
    public async Task<string> SearchAsync(string searchTerm, int results = 100)
    {
        var url = "search"
            .AddQueryParameter("num", results)
            .AddQueryParameter("q", searchTerm);

        var response = await httpClient.GetAsync(url);
        
        // Need to try catch instead? Or move try catch to scraper
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync()
;    }
}