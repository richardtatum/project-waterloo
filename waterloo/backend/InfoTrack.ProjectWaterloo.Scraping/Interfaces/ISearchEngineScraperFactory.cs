namespace InfoTrack.ProjectWaterloo.Scraping.Interfaces;

public interface ISearchEngineScraperFactory
{
    ISearchEngineScraper Create();
    bool IsInstanceOf(Type type);
}