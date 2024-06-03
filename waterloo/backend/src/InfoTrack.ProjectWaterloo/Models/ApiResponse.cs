namespace InfoTrack.ProjectWaterloo.Models;

public class ApiResponse
{
    public bool Success { get; set; } = true;
    public string[] Errors { get; set; } = [];
}