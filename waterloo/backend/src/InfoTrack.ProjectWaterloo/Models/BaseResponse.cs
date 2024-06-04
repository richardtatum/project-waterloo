namespace InfoTrack.ProjectWaterloo.Models;

public class BaseResponse
{
    public bool Success { get; set; } = true;
    public string[] Errors { get; set; } = [];
}