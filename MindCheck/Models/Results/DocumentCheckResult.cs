using MindCheck.AiSearch;
using MindCheck.Interfaces;

namespace MindCheck.Models.Results;

public class DocumentCheckResult(ICollection<AiSearchResponse> responses, ICollection<string> missingDocuments, bool hasPassed, double score) : IResult
{
    public bool HasPassed { get; set; } = hasPassed;
    public double Score { get; set; } = score;
    public ICollection<AiSearchResponse> Responses { get; set; } = responses;
    public string? Reason { get; set; }
    public int? Statements { get; set; }

    public override string ToString()
    {
        var missingDocumentsString = missingDocuments.Count != 0 ? "Missing documents: \n" + string.Join(", ", missingDocuments) : "";
        return $"DocumentCheckResult \n" +
               $"HasPassed: {HasPassed} \n"
               + missingDocumentsString;
    }
}