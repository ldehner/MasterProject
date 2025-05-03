using Microsoft.Extensions.AI;
using MindCheck.Interfaces;

namespace Client;

public class OllamaLlmCaller : ILlmCaller
{
    public async Task<string> CallLlmServiceAsync(string prompt, string system)
    {
        IChatClient chatClient =
            new OllamaChatClient(new Uri("http://localhost:11434/"), "gemma3:1b");
        var response = await chatClient.GetResponseAsync(system + "\n\n" + prompt);
        
        return response.Text;
    }
}