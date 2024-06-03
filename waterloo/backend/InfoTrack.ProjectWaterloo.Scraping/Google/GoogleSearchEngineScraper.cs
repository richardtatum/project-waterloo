using System.Text.RegularExpressions;
using System.Web;
using InfoTrack.ProjectWaterloo.Scraping.Interfaces;
using InfoTrack.ProjectWaterloo.Scraping.Models;

namespace InfoTrack.ProjectWaterloo.Scraping.Google;

public class GoogleSearchEngineScraper(GoogleClient client) : ISearchEngineScraper
{
    // Future Improvement: Load this via IOptions
    // For Google, we extract the q parameter of the first a href within the first div as these (currently) represent each result
    private readonly string _searchEntryRegexPattern = @"<div class=""[^""]+""><a href=""/url\?q=([^""]+)""";

    public async Task<SearchRanking[]> GetResultPositionsAsync(string searchTerm, string matchingDomain, int results = 100)
    {
        // Normalise the arguments
        searchTerm = searchTerm.ToLower();
        matchingDomain = matchingDomain.ToLower();
        
        if (!matchingDomain.StartsWith("http://") && !matchingDomain.StartsWith("https://"))
        {
            matchingDomain = "https://" + matchingDomain;
        }

        // Move this to a function, make tests for it
        if (!IsValidUrl(matchingDomain, out var matchingUri) || matchingUri is null)
        {
            throw new InvalidOperationException("Invalid URL provided.");
        }

        var rawHtml = await client.SearchAsync(searchTerm, results);
        return ExtractResultPositions(rawHtml, matchingUri);
    }

    private SearchRanking[] ExtractResultPositions(string rawHtml, Uri matchingUri)
    {
        // Pass the regexPattern as an argument for easier testing? 
        var matches = Regex.Matches(rawHtml, _searchEntryRegexPattern);
        if (matches.Count == 0)
        {
            throw new InvalidOperationException(
                "Unable to match any patterns for this search engine. Something may be blocking the scraping.");
        }

        return matches.Select((match, index) =>
            {
                // Groups[1] is the value captured by the parenthesis in the regex pattern (i.e. the domain url)
                var link = match.Groups[1].Value;
                if (!IsValidUrl(link, out var uri) || uri is null)
                {
                    return null;
                }

                // This match includes subdomains, so google.com will not match with www.google.com
                return uri.Host.Equals(matchingUri.Host, StringComparison.OrdinalIgnoreCase)
                    ? new SearchRanking(uri.AbsoluteUri, index + 1)
                    : null;
            })
            .Where(result => result != null)
            .ToArray()!;
    }

    private static bool IsValidUrl(string url, out Uri? validUrl)
    {
        validUrl = null;

        // Strip any google AMP additions
        url = url.Split("&amp;")[0];
        
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        validUrl = uri;
        return true;
    }
}