using System.Net;
using InfoTrack.ProjectWaterloo.Scraping.Clients;
using InfoTrack.ProjectWaterloo.Scraping.Factories;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InfoTrack.ProjectWaterloo.Scraping.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogle(this IServiceCollection services)
    {
        // This may already be registered if there are multiple search engines registered
        services.TryAddScoped<ISearchEngineScraperStrategy, SearchEngineScraperStrategy>();

        // Change URI to IOptions?
        var googleUri = new Uri("https://www.google.com");
        services.AddHttpClient<GoogleClient>(client => client.BaseAddress = googleUri)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = new CookieContainer()
                };

                // These cookies allow us to bypass the 'accept cookies before continue' screen Google now shows
                handler.CookieContainer.Add(googleUri, new Cookie("CONSENT", "PENDING+987"));
                handler.CookieContainer.Add(googleUri, 
                    new Cookie("SOCS", "CAESHAgBEhJnd3NfMjAyMzA4MTAtMF9SQzIaAmRlIAEaBgiAo_CmBg"));

                return handler;
            });

        services.AddScoped<ISearchEngineScraperFactory, GoogleSearchEngineScraperFactory>();

        return services;
    }
}