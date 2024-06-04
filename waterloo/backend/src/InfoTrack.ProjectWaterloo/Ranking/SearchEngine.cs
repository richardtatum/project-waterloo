using System.Text.Json.Serialization;

namespace InfoTrack.ProjectWaterloo.Ranking;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchEngine
{
    Google
}