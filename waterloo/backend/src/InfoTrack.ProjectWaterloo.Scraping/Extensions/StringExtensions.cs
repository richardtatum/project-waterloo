using Microsoft.AspNetCore.WebUtilities;

namespace InfoTrack.ProjectWaterloo.Scraping.Extensions;

internal static class StringExtensions
{
    internal static string AddQueryParameter(this string uri, string name, object? value)
        => QueryHelpers.AddQueryString(uri, name, value?.ToString() ?? string.Empty);
}