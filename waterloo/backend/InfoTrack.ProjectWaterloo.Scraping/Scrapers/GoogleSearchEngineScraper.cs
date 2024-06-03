using System.Text.RegularExpressions;
using InfoTrack.ProjectWaterloo.Scraping.Clients;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Models;

namespace InfoTrack.ProjectWaterloo.Scraping.Scrapers;

// TODO: Does this become results position scraper instead? Otherwise regex pattern is specific to the function
public class GoogleSearchEngineScraper(GoogleClient client) : ISearchEngineScraper
{
    // TODO: Load this via IOptions
    // For Google, we extract the q parameter of the first a href within the first div as these represent each search result
    private readonly string _regexPattern = @"<div class=""[^""]+""><a href=""/url\?q=([^""]+)""";

    public async Task<SearchRanking[]> GetResultPositionsAsync(string searchTerm, string matchingDomain, int results = 100)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }
        
        if (!matchingDomain.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || !matchingDomain.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            matchingDomain = "https://" + matchingDomain;
        }

        // Move this to a function, make tests for it
        if (!(Uri.TryCreate(matchingDomain, UriKind.Absolute, out var domainUri) && (domainUri.Scheme != Uri.UriSchemeHttp || domainUri.Scheme != Uri.UriSchemeHttps)))
        {
            return [];
        }

        var rawHtml = await client.SearchAsync(searchTerm, results);
        return ExtractResultPositions(rawHtml, domainUri);
    }

    private SearchRanking[] ExtractResultPositions(string rawHtml, Uri matchingDomain)
    {
        // Pass the regexPattern as an argument for easier testing? 
        var matches = Regex.Matches(rawHtml, _regexPattern);
        if (matches.Count == 0)
        {
            // TODO: Update error message!
            throw new InvalidOperationException(
                "Unable to match any patterns for this search engine. Potential scraping issue.");
        }

        return matches.Select((match, index) =>
            {
                // Groups[1] is the value captured by the parenthesis in the regex pattern (i.e. the domain url)
                if (!Uri.TryCreate(match.Groups[1].Value, UriKind.Absolute, out var domain))
                {
                    return null;
                }

                if (domain.Scheme != Uri.UriSchemeHttp && domain.Scheme != Uri.UriSchemeHttps)
                {
                    return null;
                }

                // TODO: WARN ABOUT SUBDOMAINS NOT MATCHING
                return domain.Host.Equals(matchingDomain.Host, StringComparison.OrdinalIgnoreCase)
                    ? new SearchRanking(domain.AbsoluteUri, index + 1)
                    : null;
            })
            .Where(ranking => ranking != null)
            .ToArray()!; // TODO: Is this suppressor a bad idea?
    }
}