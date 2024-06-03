namespace InfoTrack.ProjectWaterloo.Scraping.Models;

public class SearchRanking(string url, int position)
{
    public string Url { get; set; } = url;
    public int Position { get; set; } = position;
}