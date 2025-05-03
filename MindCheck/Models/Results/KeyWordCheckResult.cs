using MindCheck.Interfaces;

namespace MindCheck.Models.Results
{
    public class KeyWordCheckResult(
        double percent,
        int passedKeywords,
        int generalKeywords,
        ICollection<string>? missingKeywords)
        : IResult
    {
        public bool HasPassed { get; set; } = percent >= 70;
        public double Score { get; set; } = percent/100;
        private int PassedKeywords { get; set; } = passedKeywords;
        private int GeneralKeywords { get; set; } = generalKeywords;
        private ICollection<string>? MissingKeywords { get; set; } = missingKeywords;
        public string? Reason { get; set; }
        public int? Statements { get; set; }

        public override string ToString()
        {
            return $"KeyWordCheckResult \n" +
                $"HasPassed: {HasPassed} \n" +
                $"Score: {Score}% \n" +
                $"Passed Keywords: {PassedKeywords} / {GeneralKeywords} \n" +
                $"MissingKeywords: { (MissingKeywords == null ? "none" : string.Join(", ", MissingKeywords))}";
        }
    }
}
