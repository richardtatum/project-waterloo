using InfoTrack.ProjectWaterloo.Models;
using InfoTrack.ProjectWaterloo.Scraping.Extensions;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Scrapers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    // Do more validation
    if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(matchingDomain))
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

    var scraper = scraperStrategy.Create(scraperType);
    var positions = await scraper.GetResultPositionsAsync(searchTerm, matchingDomain, results);

    return Results.Ok(positions);
});

app.Run();