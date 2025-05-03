using MindCheck.Interfaces;
using MindCheck.Models.Results;

namespace MindCheck.Checks;

public abstract class BaseCheck(string answer) : ICheck
{
    private ICheck? _nextCheck;
    protected string Answer { get; set; } = answer;

    public BaseCheck() : this(null)
    {
    }

    public ICollection<IResult> Execute()
    {
        var result = RunCheck();
        var results = new List<IResult>();
        if (result.GetType() != typeof(SkipResult)) results.Add(result);

        if (_nextCheck != null) results.AddRange(_nextCheck.Execute());

        return results;
    }

    public ICheck Then(ICheck check)
    {
        if (check is BaseCheck baseCheck) baseCheck.Answer = Answer;

        if (_nextCheck == null)
        {
            _nextCheck = check;
        }
        else
        {
            var current = _nextCheck;
            while (current is BaseCheck currentBaseCheck && currentBaseCheck._nextCheck != null)
            {
                current = currentBaseCheck._nextCheck;
            }

            if (current is BaseCheck lastBaseCheck)
            {
                lastBaseCheck._nextCheck = check;
            }
        }

        return this;
    }

    protected abstract IResult RunCheck();
}