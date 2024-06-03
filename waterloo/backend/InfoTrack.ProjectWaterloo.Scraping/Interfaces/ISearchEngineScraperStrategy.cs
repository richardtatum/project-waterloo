namespace InfoTrack.ProjectWaterloo.Scraping.Interfaces;

public interface ISearchEngineScraperStrategy
{
    ISearchEngineScraper Create(Type type);
}