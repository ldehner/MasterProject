using MindCheck.AiSearch;
using MindCheck.Interfaces;
using MindCheck.Models.Results;

namespace MindCheck.Checks;

public class DocumentCheck(AiSearchType searchType, ICollection<string> requiredDocuments) : BaseCheck
{
    protected override IResult RunCheck()
    {
        IAiSearch aiSearch;
        double multiplier;
        switch (searchType)
        {
            case AiSearchType.EmbeddingSearch:
                aiSearch = new AiEmbeddingSearch();
                multiplier = 4.0;
                break;
            case AiSearchType.TextSearch:
                aiSearch = new AiTextSearch();
                multiplier = 1.0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
        }

        var responses = aiSearch.SearchAsync(Answer).Result;

        bool hasPassed;
        double score;
        var missingDocuments = requiredDocuments
            .Except(responses
                .Where(response => response.Score > 14*multiplier)
                .Select(response => response.DocumentName))
            .ToList();
        if (missingDocuments.Count > 0)
        {
            hasPassed = false;
            score = (requiredDocuments.Count - missingDocuments.Count) / (double)requiredDocuments.Count;
        }
        else
        {
            score = 1.0;
            hasPassed = true;
        }
        
        return new DocumentCheckResult(responses, missingDocuments, hasPassed, score);
    }
}