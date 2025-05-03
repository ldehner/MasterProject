using MindCheck.Interfaces;

namespace MindCheck.Models.Results.LLM;

public class HallucinationCheckResult: IResult
{
    public bool HasPassed { get; set; }
    public double Score { get; set; }
    public string? Reason { get; set; }
    public int? Statements { get; set; }

    public override string ToString()
    {
        return $"ContextCheckResult \n" +
            $"HasPassed: {HasPassed} \n" +
            $"Score: {Score}";
    }
}