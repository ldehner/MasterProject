using MindCheck.Interfaces;

namespace MindCheck.Models.Results;

public class LlmErrorResult : IResult
{
    public bool HasPassed { get; set; } = false;
    public double Score { get; set; } = 0;
    public string? Reason { get; set; } = "The LLM returned an unexpected response format.";
    public int? Statements { get; set; }

    public override string ToString()
    {
        return "The LLM returned an unexpected response format. " +
               "Please check the LLM configuration and ensure that the response format is correct.";
    }
}