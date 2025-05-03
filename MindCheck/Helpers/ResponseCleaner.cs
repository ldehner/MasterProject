namespace MindCheck.Helpers;

public static class ResponseCleaner
{
    public static string Cleanup(string input)
    {
        var start = input.IndexOf('[');
        var end = input.IndexOf(']');
        return input[start..(end + 1)];
    }
}