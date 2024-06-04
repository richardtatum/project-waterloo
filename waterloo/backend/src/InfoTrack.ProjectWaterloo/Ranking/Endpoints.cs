using InfoTrack.ProjectWaterloo.Scraping.Google;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.ProjectWaterloo.Ranking;

public static class Endpoints
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
            return Results.BadRequest(new Response("One or more parameters are missing, empty or invalid."));
        }

        var scraperType = searchEngine switch
        {
            SearchEngine.Google => typeof(SearchEngineScraper),
            _ => null
        };

        if (scraperType is null)
        {
            return Results.BadRequest(new Response("Search Engine is not registered for scraping."));
        }

        try
        {
            var scraper = scraperStrategy.Create(scraperType);
            var positions = await scraper.GetResultPositionsAsync(searchTerm, matchingDomain, results);
            return Results.Ok(new Response(positions));
        }
        catch (InvalidOperationException ex)
        {
            return Results.UnprocessableEntity(
                new Response(ex.Message));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Results.UnprocessableEntity(
                new Response(
                    "Failed to create scraper for requested search engine. Please contact an administrator."));
        }
    }
}