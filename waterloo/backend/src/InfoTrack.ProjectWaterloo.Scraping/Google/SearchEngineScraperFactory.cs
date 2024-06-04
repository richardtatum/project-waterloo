using InfoTrack.ProjectWaterloo.Scraping.Interfaces;

namespace InfoTrack.ProjectWaterloo.Scraping.Google;

internal class SearchEngineScraperFactory(Client client) : ISearchEngineScraperFactory
{
    private SearchEngineScraper? _scraper;

    public ISearchEngineScraper Create()
    {
        return _scraper ??= new SearchEngineScraper(client);
    }

    public bool IsTypeOf(Type type) => typeof(SearchEngineScraper) == type;
}