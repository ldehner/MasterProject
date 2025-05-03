using System.Text.Json;
using MindCheck.Helpers;
using MindCheck.Interfaces;
using MindCheck.Models;
using MindCheck.Models.Responses;
using MindCheck.Models.Results;
using MindCheck.Models.Results.LLM;

namespace MindCheck.Checks.LLM
{
    public class ContextualRelevancyCheck(
        string input,
        ILlmCaller llmCaller,
        string systemInformation = ContextualRelevancyCheck.DefaultSystemInformation,
        int? statementCount = null,
        double threshold = 0.7)
        : BaseCheck
    {
        private const string DefaultSystemInformation = "You are a context detection model. Make sure to read in all statements from the CONTEXT";
        private const string ResponseFormat = """
            Your only job is to separate the CONTEXT into statements, 
            and then determine if the seperated statement is relevant to the provided INPUT. Return only a JSON without any other text.
            Make sure your response starts with [ and ends with ].
            The JSON should be structured as follows:
            
            [
                {
                    "Statement": "This is the first statement.",
                    "IsRelevant": true
                },
                {
                    "Statement": "This is the second statement.",
                    "IsRelevant": false
                },
                {
                    "Statement": "This is the third statement.",
                    "IsRelevant": true
                }
            ]
            """;
        private string _systemInformation { get; set; } = systemInformation + "\n" + ResponseFormat;
        private int? _statementCount { get; set; } = statementCount;
        private string _input { get; set; } = input;
        private double _threshold { get; set; } = threshold;

        protected override IResult RunCheck()
        {
            var checkerPrompt = "CONTEXT:\n" + Answer + "\nINPUT:\n" + _input;
            var response = llmCaller.CallLlmServiceAsync(checkerPrompt, _systemInformation).Result;
            List<ContextRelevancyModel>? result;
            try
            {
                result = JsonSerializer.Deserialize<List<ContextRelevancyModel>>(ResponseCleaner.Cleanup(response));
            }catch (Exception)
            {
                return new LlmErrorResult();
            }
            if (result == null || result.Count == 0)
            {
                return new LlmErrorResult();
            }

            var relevant = result.Count(x => x.IsRelevant);
            var all = result.Count;
            var score = (double)relevant / all;
            if (_statementCount != null && all != _statementCount)
            {
                return new ContextualRelevancyCheckResult
                {
                    Score = score,
                    HasPassed = false,
                    Statements = all,
                    Reason = $"Expected {_statementCount} statements, but got {all}."
                };
            }
            return new ContextualRelevancyCheckResult
            {
                Score = score,
                HasPassed = score >= _threshold,
                Statements = all,
                Reason = score >= _threshold ? "The score is above the threshold." : "The score is below the threshold.",
            };
        }
    }
}
