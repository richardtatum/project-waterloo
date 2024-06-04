using InfoTrack.ProjectWaterloo.Models;
using InfoTrack.ProjectWaterloo.Scraping.Models;

namespace InfoTrack.ProjectWaterloo.Ranking;

public class Response : BaseResponse
{
    public Response(string error)
    {
        Success = false;
        Errors = [error];
    }

    public Response(SearchRanking[] rankings)
    {
        Rankings = rankings;
    }

    public SearchRanking[] Rankings { get; set; } = [];
}