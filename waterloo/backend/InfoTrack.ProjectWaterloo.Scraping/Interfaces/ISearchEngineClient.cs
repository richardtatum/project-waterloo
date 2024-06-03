namespace InfoTrack.ProjectWaterloo.Scraping.Interfaces;

public interface ISearchEngineClient
{
    Task<string> SearchAsync(string searchTerm, int results = 100);
}