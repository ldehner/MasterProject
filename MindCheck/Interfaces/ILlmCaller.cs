namespace MindCheck.Interfaces;

public interface ILlmCaller
{
    Task<string> CallLlmServiceAsync(string prompt, string system);
}