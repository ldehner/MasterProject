using System.Net.Http.Json;
using System.Text.Json.Serialization;
using MindCheck.Interfaces;
using Newtonsoft.Json;

namespace MindCheck.AiSearch;

public class AiEmbeddingSearch : IAiSearch
{
    private static readonly string
        deploymentName = "text-embedding-3-small";

    private static readonly string apiVersion = "2024-02-01"; 
    
    public async Task<ICollection<AiSearchResponse>> SearchAsync(string text)
    {
        var embeddings = await GetEmbeddingsAsync(text);
        var searchResults = await SearchDocumentByVectorAsync(embeddings);
        return searchResults;
    }
    
    private static async Task<float[]> GetEmbeddingsAsync(string inputText)
    {
        var apiKey = "censored";
        var endpoint = "https://censored.openai.azure.com/";
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
        {
            throw new Exception("API-KEY or API-URL environment variable is missing.");
        }

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        // Build the embeddings endpoint URL
        var url = $"{endpoint}openai/deployments/{deploymentName}/embeddings?api-version={apiVersion}";

        // Prepare the request body
        var requestBody = new
        {
            input = inputText // Input text for which to generate embeddings
        };

        // Send the POST request to the embeddings endpoint
        var response = await client.PostAsJsonAsync(url, requestBody);

        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response into a strongly typed object
            var responseBody = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
            return responseBody?.Data?[0]?.Embedding ?? throw new Exception("No embedding returned.");
        }
        else
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error: {errorBody}");
        }
    }
    
     private static async Task<ICollection<AiSearchResponse>> SearchDocumentByVectorAsync(float[] queryEmbedding)
    {
        // Replace with your actual service settings.
        const string serviceName = "censored";
        const string indexName = "vector-index";
        const string apiKey = "censored";
        const string apiVersion = "2024-07-01";         // API version supporting vector search

        // Build the base URI (make sure it ends with a slash)
        string baseUri = $"https://{serviceName}.search.windows.net/";
        // Build the full URL for the search endpoint
        string url = $"{baseUri}indexes/{indexName}/docs/search?api-version={apiVersion}";

        // Build the payload with the expected property names and structure.
        // Note: We use "vector" (not "queryVector") inside the vectorQueries array.
        var payload = new
        {
            search = "", // empty because the search is solely based on vector similarity
            vectorQueries = new[]
            {
                new
                {
                    kind = "vector",          // Specify that this is a vector query
                    vector = queryEmbedding,    // The query embedding (an array of floats)
                    fields = "text_vector",     // The name of your vector field in the index
                    k = 5,                    // How many nearest neighbors to retrieve
                    exhaustive = true         // Optional: enforce exhaustive KNN search
                }
            },
            top = 5,                       // Limit the number of returned documents
            // Use a comma-separated string per API expectations.
            select = "chunk_id, title"
        };

        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

            // Post the JSON payload to the search endpoint.
            var response = await httpClient.PostAsJsonAsync(url, payload);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<SearchResult>(responseContent);
                var aiSearchResponse = responseModel.Value.Select(result => new AiSearchResponse { Score = result.SearchScore * 100, DocumentName = result.Title }).ToList();
                return aiSearchResponse;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}\nDetails: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Vector search call failed", ex);
        }
    }
}

internal class SearchResult
{
    [JsonProperty("@odata.context")]
    public string OdataContext { get; set; }

    [JsonProperty("value")]
    public List<SearchValue> Value { get; set; }
}

internal class SearchValue
{
    [JsonProperty("@search.score")]
    public float SearchScore { get; set; }

    [JsonProperty("chunk_id")]
    public string ChunkId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
}

internal class EmbeddingResponse
{
    [JsonPropertyName("data")] public EmbeddingData[] Data { get; set; }
}

internal class EmbeddingData
{
    [JsonPropertyName("embedding")] public float[] Embedding { get; set; }
}