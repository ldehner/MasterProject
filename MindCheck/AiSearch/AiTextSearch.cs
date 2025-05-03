using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using MindCheck.Interfaces;

namespace MindCheck.AiSearch;

public class AiTextSearch : IAiSearch
{
    public async Task<ICollection<AiSearchResponse>> SearchAsync(string text)
    {
        // Replace with your service details
        const string serviceName = "censored";
        const string indexName = "vector-index";
        const string apiKey = "censored";
        Console.WriteLine("Searching for AI search documents...");
        // Create a search client
        var endpoint = new Uri($"https://{serviceName}.search.windows.net/");
        var client = new SearchClient(endpoint, indexName, new AzureKeyCredential(apiKey));

        // Configure search options
        var options = new SearchOptions
        {
            IncludeTotalCount = true, // Enables TotalCount property
            Size = 10, // Limit results to 10
        };

        ICollection<AiSearchResponse> aiSearchResponse = null;
        // Execute the search query
        try
        {
            Response<SearchResults<SearchDocument>> response =
                client.Search<SearchDocument>(text, options);


            // Access total count of results (nullable)
            if (response.Value.TotalCount.HasValue)
            {
                Console.WriteLine($"Total Results: {response.Value.TotalCount.Value}");
            }
            else
            {
                Console.WriteLine("Total count not available.");
            }
            aiSearchResponse = response.Value.GetResults().Select(result => new AiSearchResponse { Score = (double)result.Score!, DocumentName = (result.Document["title"] as string)! }).ToList();
            // Iterate over results
            foreach (var result in response.Value.GetResults())
            {
                Console.WriteLine($"Document ID: {result.Document["chunk_id"]}");
                Console.WriteLine($"Title: {result.Document["title"]}");
                Console.WriteLine($"(Score: {result.Score})");
                //Console.WriteLine($"Title: {result.Document["metadata_storage_name"]}");
                Console.WriteLine();
            }

            // response.Value.GetResults().ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //return ex.Message;
        }
        return aiSearchResponse;
    }
}