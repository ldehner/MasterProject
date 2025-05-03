
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MindCheck.Interfaces;

namespace Client
{
    public class AzureLlmCaller : ILlmCaller
    { 
        private static readonly string deploymentName = "gpt-35-turbo"; // E.g., "gpt-35-turbo"
        private static readonly string apiVersion = "2024-10-01-preview"; // Check for the correct version in Azure documentation
        public async Task<string> CallLlmServiceAsync(string prompt, string system)
        {
            var apiKey = Environment.GetEnvironmentVariable("API-KEY");
            var endpoint = Environment.GetEnvironmentVariable("API-URL");
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("API-KEY or API-URL environment variable is missing.");
            }
            await Task.Delay(20 * 1000); // needed because of request limit
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("api-key", apiKey);

            var url = $"{endpoint}openai/deployments/{deploymentName}/chat/completions?api-version={apiVersion}";

            var requestBody = new
            {
                messages = new[]
                {
                new { role = "system", content = system },
                new { role = "user", content = prompt }
            },
                max_tokens = 500,
                temperature = 0.7
            };

            var response = await client.PostAsJsonAsync(url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                return responseBody?.Choices?[0]?.Message?.Content ?? "No response from model.";
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {errorBody}");
            }
        }
    }
    public class OpenAIResponse
    {
        [JsonPropertyName("choices")]
        public Choice[] Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
