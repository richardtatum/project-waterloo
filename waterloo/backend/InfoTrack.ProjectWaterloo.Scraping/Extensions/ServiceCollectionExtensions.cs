using System.Net;
using InfoTrack.ProjectWaterloo.Scraping.Clients;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Scrapers;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.ProjectWaterloo.Scraping.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogle(this IServiceCollection services)
    {
        // Change URI to IOptions?
        var googleUri = new Uri("https://www.google.com");
        services.AddHttpClient<GoogleClient>(client => client.BaseAddress = googleUri)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = new CookieContainer()
                };

                handler.CookieContainer.Add(googleUri, new Cookie("CONSENT", "PENDING+987"));
                handler.CookieContainer.Add(googleUri, 
                    new Cookie("SOCS", "CAESHAgBEhJnd3NfMjAyMzA4MTAtMF9SQzIaAmRlIAEaBgiAo_CmBg"));

                return handler;
            });
        services.AddScoped<IRankingScraper, GoogleRankingScraper>();

        return services;
    }
}