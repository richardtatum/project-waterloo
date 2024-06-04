using InfoTrack.ProjectWaterloo.Models;
using InfoTrack.ProjectWaterloo.Scraping.Models;

namespace InfoTrack.ProjectWaterloo.Ranking;

public class RankingResponse : ApiResponse
{
    public RankingResponse(string error)
    {
        Success = false;
        Errors = [error];
    }

    public RankingResponse(SearchRanking[] rankings)
    {
        Rankings = rankings;
    }

    public SearchRanking[] Rankings { get; set; } = [];
}