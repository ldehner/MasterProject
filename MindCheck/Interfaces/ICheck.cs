namespace MindCheck.Interfaces;

public interface ICheck
{
    ICheck Then(ICheck check);
    ICollection<IResult> Execute();
}