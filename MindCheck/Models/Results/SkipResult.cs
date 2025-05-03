using MindCheck.Interfaces;

namespace MindCheck.Models.Results
{
    public class SkipResult : IResult
    {
        public bool HasPassed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Reason { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? Statements { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
