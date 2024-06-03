namespace InfoTrack.ProjectWaterloo.Scraping.Interfaces;

public interface ISearchEngineScraperFactory
{
    ISearchEngineScraper Create();
    bool IsTypeOf(Type type);
}