namespace MindCheck.Models.Responses;

public class ContextRelevancyModel
{
    public required string Statement { get; set; }
    public required bool IsRelevant { get; set; }
}