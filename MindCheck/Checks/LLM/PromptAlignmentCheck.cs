using MindCheck.Interfaces;
using MindCheck.Models.Results;
using MindCheck.Models.Results.LLM;

namespace MindCheck.Checks.LLM;

public class PromptAlignmentCheck(
        string input,
        ICollection<string> instructions,
        ILlmCaller llmCaller,
        string systemInformation = PromptAlignmentCheck.DefaultSystemInformation,
        double threshold = 0.7)
        : BaseCheck
    {
        private const string DefaultSystemInformation = "You are a prompt alignment check model. Make sure to read all the instructions, input and output.";
        private const string ResponseFormat = """
            Your task is to evaluate if the provided instructions were followed. Check if the output generated in response to a question followed the instructions. Follow these steps:
            
            1. Carefully read and understand the provided instructions.
            2. Analyze the output to identify if all the instructions were followed.
            3. Count the number of instructions followed.
            4. Return only a single number representing the count of followed instructions, starting from 0.
            
            Do not include any explanations, text, or additional information—only return the number.
            """;
        private string systemInformation { get; set; } = systemInformation + "\n" + ResponseFormat;

        protected override IResult RunCheck()
        {
            var checkerPrompt = "CONTEXT:\n" + string.Join('\n', instructions) + "\n\nINPUT:\n" + input + "\nOUTPUT:\n" + Answer;;
            var response = llmCaller.CallLlmServiceAsync(checkerPrompt, systemInformation).Result;
            Console.WriteLine(response);
            int result;
            try
            {
                result = int.Parse(response);
            }catch (Exception)
            {
                return new LlmErrorResult();
            }
            var all = instructions.Count;
            var score = result == 0 ? 1 : (double)result / all;
          
            return new PromptAlignmentCheckResult()
            {
                Score = score,
                HasPassed = score >= threshold,
            };
        }
    }