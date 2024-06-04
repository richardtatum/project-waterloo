using InfoTrack.ProjectWaterloo.Scraping.Google;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.ProjectWaterloo.Ranking;

public static class RankingModule
{
    public static void AddRankingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api");
        group.MapGet("ranking", GetAsync);
    }

    private static async Task<IResult> GetAsync(ISearchEngineScraperStrategy scraperStrategy, string? searchTerm,
        [FromQuery] SearchEngine? searchEngine, string? matchingDomain, int results = 100)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(matchingDomain) || results <= 0)
        {
            return Results.BadRequest(new RankingResponse("One or more parameters are missing, empty or invalid."));
        }

        var scraperType = searchEngine switch
        {
            SearchEngine.Google => typeof(GoogleSearchEngineScraper),
            _ => null
        };

        if (scraperType is null)
        {
            return Results.BadRequest(new RankingResponse("Search Engine is not registered for scraping."));
        }

        try
        {
            var scraper = scraperStrategy.Create(scraperType);
            var positions = await scraper.GetResultPositionsAsync(searchTerm, matchingDomain, results);
            return Results.Ok(new RankingResponse(positions));
        }
        catch (InvalidOperationException ex)
        {
            return Results.UnprocessableEntity(
                new RankingResponse(ex.Message));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Results.UnprocessableEntity(
                new RankingResponse(
                    "Failed to create scraper for requested search engine. Please contact an administrator."));
        }
    }
}