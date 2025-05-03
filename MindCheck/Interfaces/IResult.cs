namespace MindCheck.Interfaces;

public interface IResult
{
    public bool HasPassed { get; set; }
    public double Score { get; set; }
    public string? Reason { get; set; }
    public int? Statements { get; set; }
}