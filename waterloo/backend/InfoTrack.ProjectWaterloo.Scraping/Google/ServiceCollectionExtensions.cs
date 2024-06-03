using System.Net;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InfoTrack.ProjectWaterloo.Scraping.Google;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogle(this IServiceCollection services)
    {
        // This may already be registered if there are multiple search engines registered
        services.TryAddScoped<ISearchEngineScraperStrategy, SearchEngineScraperStrategy>();

        services.AddScoped<ISearchEngineScraperFactory, GoogleSearchEngineScraperFactory>();

        // Future Improvement: Use IOptions to load this value
        var url = new Uri("https://www.google.com");
        services.AddHttpClient<GoogleClient>(client => client.BaseAddress = url)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();

                // These cookies allow us to bypass the 'accept cookies before continue' screen Google now shows
                handler.CookieContainer.Add(url, new Cookie("CONSENT", "PENDING+987"));
                handler.CookieContainer.Add(url, 
                    new Cookie("SOCS", "CAESHAgBEhJnd3NfMjAyMzA4MTAtMF9SQzIaAmRlIAEaBgiAo_CmBg"));

                return handler;
            });

        return services;
    }
}