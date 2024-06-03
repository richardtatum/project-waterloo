using InfoTrack.ProjectWaterloo.Scraping.Interfaces;

namespace InfoTrack.ProjectWaterloo.Scraping.Strategies;

// DI will resolve all instances of the ISearchEngineScraperFactory that have been registered
public class SearchEngineScraperStrategy(IEnumerable<ISearchEngineScraperFactory> searchEngineScraperFactories)
    : ISearchEngineScraperStrategy
{
    public ISearchEngineScraper Create(Type type)
    {
        var factory = searchEngineScraperFactories.FirstOrDefault(scraper => scraper.IsTypeOf(type));
        if (factory is null)
        {
            throw new ArgumentOutOfRangeException(nameof(type), $"Factory of type {type} is not registered.");
        }

        return factory.Create();
    }
}