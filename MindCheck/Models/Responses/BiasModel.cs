namespace MindCheck.Models.Responses;

public class BiasModel
{
    public required string Opinion { get; set; }
    public required bool IsBiased { get; set; }
    public string? BiasType { get; set; }
}