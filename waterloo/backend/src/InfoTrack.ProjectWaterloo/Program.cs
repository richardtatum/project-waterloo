using InfoTrack.ProjectWaterloo.Models;
using InfoTrack.ProjectWaterloo.Scraping.Google;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGoogle();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ranking", async (ISearchEngineScraperStrategy scraperStrategy, string searchTerm, [FromQuery] SearchEngine searchEngine, string matchingDomain, int results = 100) =>
{
    if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(matchingDomain) || results <= 0)
    {
        return Results.BadRequest();
    }

    var scraperType = searchEngine switch
    {
        SearchEngine.Google => typeof(GoogleSearchEngineScraper),
        _ => null
    };

    if (scraperType is null)
    {
        return Results.BadRequest("Search engine is not registered.");
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
            new RankingResponse("Failed to scrape requested search engine. Please contact an administrator."));
    }
    catch (ArgumentOutOfRangeException ex)
    {
        return Results.UnprocessableEntity(
            new RankingResponse(
                "Failed to create scraper for requested search engine. Please contact an administrator."));
    }

});

app.Run();