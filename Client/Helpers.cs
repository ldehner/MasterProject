using MindCheck.Interfaces;

namespace Client;

public static class Helpers
{
    public static List<Tuple<bool, double, string, int>> ExtractResults(ICollection<IResult> results)
    {
        return results.Select(result => new Tuple<bool, double, string, int>(result.HasPassed, result.Score, result.Reason ?? "", result.Statements ?? -1)).ToList();
    }
    
    public static void PrintResults(List<Tuple<bool, double, string, int>> results, string type)
    {
        Console.WriteLine(type);
        foreach (var r in results)
        {
            Console.WriteLine($"{r.Item1} {r.Item2} {r.Item3} {r.Item4}");
        }
    }

    public static void PrintResults(ICollection<IResult> results)
    {
        foreach (var r in results) Console.WriteLine(r + "\n");
    }
}

public enum LlmType
{
    OpenAi,
    Azure,
    Ollama
}

public enum OutputType
{
    Excel,
    Console
}