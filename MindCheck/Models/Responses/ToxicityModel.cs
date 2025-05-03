namespace MindCheck.Models.Responses;

public class ToxicityModel
{
    public required string Opinion { get; set; }
    public required bool IsToxic { get; set; }
    public string? Reason { get; set; }
}