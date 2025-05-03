using System.Text.Json;
using Client.Mocks;
using MindCheck.AiSearch;
using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;
using MindCheck.Models;

namespace Client;

public static class Start
{
    private static void Main(string[] args)
    {
        DocumentMock.Run(OutputType.Excel, AiSearchType.EmbeddingSearch);
    }
}