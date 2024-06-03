namespace InfoTrack.ProjectWaterloo.Models;

public class RankingRequestModel
{
    public string? SearchTerm { get; set; }
    public string? MatchingDomain { get; set; }
    public int Results { get; set; } = 100;
}