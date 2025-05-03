using MindCheck.Interfaces;

namespace MindCheck.Models.Results;

public class SyntaxCheckResult(double result) : IResult
{
    public bool HasPassed { get; set; } = result >= 0.8;
    public double Score { get; set; } = Math.Round(result * 100);
    public string? Reason { get; set; }
    public int? Statements { get; set; }
    public override string ToString()
    {
        Console.WriteLine(result);
        return nameof(SyntaxCheckResult) + " \n" +
               $"HasPassed: {HasPassed} \n" +
               $"Score: {Score}% \n";
    }
}