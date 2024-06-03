using InfoTrack.ProjectWaterloo.Scraping.Interfaces;

namespace InfoTrack.ProjectWaterloo.Scraping.Google;

internal class GoogleSearchEngineScraperFactory(GoogleClient client) : ISearchEngineScraperFactory
{
    private GoogleSearchEngineScraper? _scraper;

    public ISearchEngineScraper Create()
    {
        return _scraper ??= new GoogleSearchEngineScraper(client);
    }

    public bool IsTypeOf(Type type) => typeof(GoogleSearchEngineScraper) == type;
}