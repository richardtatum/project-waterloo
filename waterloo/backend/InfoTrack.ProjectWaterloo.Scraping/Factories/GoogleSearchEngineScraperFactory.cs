using InfoTrack.ProjectWaterloo.Scraping.Clients;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Scrapers;

namespace InfoTrack.ProjectWaterloo.Scraping.Factories;

public class GoogleSearchEngineScraperFactory(GoogleClient client) : ISearchEngineScraperFactory
{
    private GoogleSearchEngineScraper? _scraper;

    public ISearchEngineScraper Create()
    {
        return _scraper ??= new GoogleSearchEngineScraper(client);
    }

    public bool IsInstanceOf(Type type) => typeof(GoogleSearchEngineScraper) == type;
}