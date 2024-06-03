using System.Text.Json.Serialization;

namespace InfoTrack.ProjectWaterloo.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchEngine
{
    Google
}