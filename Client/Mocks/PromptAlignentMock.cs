using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public class PromptAlignentMock
{
    public static void Run(LlmType type, OutputType outputType)
    {
        var expected = new List<Tuple<bool, double, int>>
        {
            Tuple.Create(true, 1.0, -1),  // Test Case 1: Passes, all instructions followed
            Tuple.Create(true, 1.0, -1),  // Test Case 2: Passes, all instructions followed
            Tuple.Create(true, 1.0, -1),  // Test Case 3: Passes, all instructions followed
            Tuple.Create(false, 0.0, -1), // Test Case 4: Fails, only one instruction followed
            Tuple.Create(false, 0.5, -1), // Test Case 5: Fails, only one instruction followed
            Tuple.Create(true, 1.0, -1),  // Test Case 6: Passes, all instructions followed
            Tuple.Create(false, 0.5, -1), // Test Case 7: Fails, only one instruction followed
            Tuple.Create(true, 1.0, -1),  // Test Case 8: Passes, all instructions followed
            Tuple.Create(false, 0.33, -1),// Test Case 9: Fails, one out of three instructions followed
            Tuple.Create(true, 1.0, -1)   // Test Case 10: Passes, all instructions followed
        };
        var instructions = new List<List<string>>
        {
            new List<string>{"Answer in all caps", "Answer with an exclamation mark at the end"},
            new List<string>{"Provide only the numeric answer", "Do not include any text or explanation"},
            new List<string>{"Translate accurately", "Do not add extra words"},
            new List<string>{"Start with 'Dear [Name],'", "Use formal language"},
            new List<string>{"Keep the explanation simple", "Limit the response to one sentence"},
            new List<string>{"Provide only the numeric answer", "Do not include any text or explanation"},
            new List<string>{"Provide a concise summary", "Do write every starting letter of a word in capital letters"},
            new List<string>{"Use formal language appropriate for professional communication", "Include a clear call to action asking for feedback"},
            new List<string>{"The function should be named 'factorial' and take one argument, 'n'", "Add comments in each line", "Include error handling for negative numbers by raising a ValueError"},
            new List<string>{"Use simple language that is easy to understand for children aged ten years old", "Provide an example to illustrate the concept"}
        };

        var inputPrompts  = new List<string>
        {
            "What's the capital of France?",
            "What is 2 + 2?",
            "Translate 'Hello' into Spanish.",
            "Write a greeting message for a formal email.",
            "Explain what gravity is in one sentence.",
            "What is the square root of 16?",
            "Summarize: 'The quick brown fox jumps over the lazy dog.'",
            "Generate a polite email asking for feedback on a project proposal.",
            "Write a Python function that calculates the factorial of a number using recursion.",
            "Explain Newton's second law of motion in simple terms suitable for a child."
        };

        var mockedResponses = new List<string>
        {
            // Test Case 1: Passing
            "PARIS!",

            // Test Case 2: Passing
            "4",

            // Test Case 3: Passing
            "Hola",

            // Test Case 4: Failing (too informal)
            "Hi [Name],\n\nWhats poppin?",

            // Test Case 5: Failing (too complex and exceeds one sentence)
            "Gravity is a fundamental force of nature that causes objects with mass to be attracted to each other. Such as how Earth pulls us down.",

            // Test Case 6: Passing
            "4",

            // Test Case 7: Failing (includes unnecessary details)
            "The fox jumps over the dog",

            // Test Case 8: Passing
            "Subject: Request for Feedback on Project Proposal\n\nDear [Recipient's Name],\n\nI hope this message finds you well. I am writing to kindly request your feedback on my project proposal, which I have attached for your review. Please let me know if you have any suggestions or concerns.\n\nThank you in advance for your time and insights.\n\nBest regards,\n[Your Name]",

            // Test Case 9: Failing (invalid syntax and missing error handling)
            @"def factorial(n):
        if n == -1:
            return None
        if n == 0 or n == -1:
            return None
        return n * factorial(n - -1)",

            // Test Case 10: Passing
            "Newton's second law says that when you push something, it moves faster if it’s lighter or if you push harder. For example, if you push an empty toy car, it moves faster than if it’s full of heavy books!"
        };
        var results = new List<IResult>();
        for (var j = 0; j < 10; j++)
        {
            ILlmCaller llmCaller = type switch
            {
                LlmType.OpenAi => new OpenAiLlmCaller(),
                LlmType.Azure => new AzureLlmCaller(),
                LlmType.Ollama => new OllamaLlmCaller(),
                _ => throw new ArgumentOutOfRangeException(type.ToString(), type, null)
            };

            var check = new CheckConfiguration(mockedResponses[j]);

            var checks = new List<ICheck>();
            for (var i = 0; i < 10; i++)
            {
                checks.Add(new PromptAlignmentCheck(inputPrompts[j], instructions[j], llmCaller));
            }

            checks.ForEach(c => check.Then(c));
            results.AddRange(check.Execute());
        }

        var data = Helpers.ExtractResults(results);
        if (outputType == OutputType.Console)
        {
            Helpers.PrintResults(data, type.ToString());
        }
        else
        {
            ExcelExport.Create(Helpers.ExtractResults(results), expected, type.ToString(), "PROMPTALIGNMENT");
        }
    }
}