using InfoTrack.ProjectWaterloo.Scraping.Models;

namespace InfoTrack.ProjectWaterloo.Scraping.Interfaces;

public interface IRankingScraper
{
    Task<SearchRanking[]> GetResultPositionsAsync(string searchTerm, string matchingDomain, int results = 100);
}