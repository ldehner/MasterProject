using MindCheck.Interfaces;
using MindCheck.Models.Results;

namespace MindCheck.Checks
{
    public class KeyWordCheck(ICollection<string> keyWords) : BaseCheck
    {
        protected override IResult RunCheck()
        {
            var count = keyWords.Count(word => Answer.Contains(word, StringComparison.OrdinalIgnoreCase));
            var percent = ((double)count / keyWords.Count) * 100;
            if (count >= keyWords.Count) return new KeyWordCheckResult(percent, count, keyWords.Count, null);
            {
                var matchedKeywords = keyWords.Where(word => Answer.Contains(word, StringComparison.OrdinalIgnoreCase)).ToList();
                var missingKeywords = keyWords.Except(matchedKeywords).ToList();
                return new KeyWordCheckResult(percent, count, keyWords.Count, missingKeywords);
            }
        }
    }
}
