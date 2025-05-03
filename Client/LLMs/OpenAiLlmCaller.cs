using MindCheck.Interfaces;
using OpenAI.Chat;

namespace Client;

public class OpenAiLlmCaller : ILlmCaller
{
    public async Task<string> CallLlmServiceAsync(string prompt, string system)
    {
        ChatClient client = new(model: "gpt-4o",
            apiKey:
            "censored");

        ChatCompletion completion = await client.CompleteChatAsync(system + "\n\n" + prompt);

        return completion.Content[0].Text;
    }
}