
using MindCheck.Interfaces;
using MindCheck.Models.Results;
using MindCheck.Models.Results.LLM;

namespace MindCheck.Checks.LLM;

public class HallucinationCheck(
        string input,
        ICollection<string> context,
        ILlmCaller llmCaller,
        string systemInformation = HallucinationCheck.DefaultSystemInformation,
        double threshold = 0.7)
        : BaseCheck
    {
        private const string DefaultSystemInformation = "You are a hallucination detection model. Make sure to read all the context, input and output.";
        private const string ResponseFormat = """
            Your task is to evaluate the provided context and the output generated in response to a question about the context. Follow these steps carefully:
            
            Understand the Context: Carefully read and fully comprehend the provided context.
            
            Split the Output: Break the output into individual statements for analysis.
            
            Check for Contradictions: Compare each statement in the output against the context to determine if it directly contradicts or conflicts with the information in the context.
            
            A contradiction occurs when a statement explicitly disagrees with or negates information in the context (e.g., "The capital of France is London" contradicts "The capital of France is Paris").
            
            If a statement is not mentioned in the context, it should be counted as contradictory (e.g., if "The Eiffel Tower is in Paris" is not stated in the context, then any related claim would be considered contradictory).
            
            Focus Only on Contradictions: Do not analyze for correctness, relevance, or completeness—focus solely on identifying contradictions.
            
            Count Contradictions: Count only the number of statements in the output that contradict or conflict with the context.
            
            Return a Single Number: Return only a single number representing the count of contradicting statements, starting from 0 if there are none.
            
            Zero for No Contradictions: If there are no contradicting statements, return 0 without any additional text or explanation.
            
            Double-Check for Accuracy: Before counting a statement as contradictory, ensure that it explicitly conflicts with information in the context or is unsupported by it.
            """;
        private string _systemInformation { get; set; } = systemInformation + "\n" + ResponseFormat;
        private string _input { get; set; } = input;
        private double _threshold { get; set; } = threshold;

        protected override IResult RunCheck()
        {
            var checkerPrompt = "CONTEXT:\n" + string.Join('\n', context) + "\n\nINPUT:\n" + _input + "\nOUTPUT:\n" + Answer;;
            var response = llmCaller.CallLlmServiceAsync(checkerPrompt, _systemInformation).Result;
            Console.WriteLine(response);
            int result;
            try
            {
                result = int.Parse(response);
            }catch (Exception)
            {
                return new LlmErrorResult();
            }
            var all = context.Count;
            var score = result == 0 ? 1 : ((double)all - result) / all;
          
            return new HallucinationCheckResult()
            {
                Score = score,
                HasPassed = score >= _threshold,
            };
        }
    }