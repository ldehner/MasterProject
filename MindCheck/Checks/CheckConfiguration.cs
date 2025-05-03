using MindCheck.Interfaces;
using MindCheck.Models.Results;

namespace MindCheck.Checks;

public class CheckConfiguration(string? answer) : BaseCheck(answer)
{
    protected override IResult RunCheck()
    {
        return new SkipResult();
    }
}