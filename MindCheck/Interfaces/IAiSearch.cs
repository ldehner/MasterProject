using MindCheck.AiSearch;

namespace MindCheck.Interfaces;

public interface IAiSearch
{
    public Task<ICollection<AiSearchResponse>> SearchAsync(string text);
}