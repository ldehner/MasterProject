using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public static class HallucinationMock
{
     public static void Run(LlmType type, OutputType outputType)
    {
        var expected = new List<Tuple<bool, double, int>> {
            Tuple.Create(true, 1.0, -1),   // Dataset 1: Pass (score 1.0, no contradictions)
            Tuple.Create(false, 0.67, -1),  // Dataset 2: Fail (score 0.5, 2 contradictions)
            Tuple.Create(true, 1.0, -1),   // Dataset 3: Pass (score 1.0, no contradictions)
            Tuple.Create(false, 0.67, -1), // Dataset 4: Fail (score 0.75, 1 contradiction)
            Tuple.Create(true, 1.0, -1),   // Dataset 5: Pass (score 1.0, no contradictions)
            Tuple.Create(false, 0.67, -1),  // Dataset 6: Fail (score 0.5, 2 contradictions)
            Tuple.Create(true, 1.0, -1),   // Dataset 7: Pass (score 1.0, no contradictions)
            Tuple.Create(false, 0.67, -1), // Dataset 8: Fail (score 0.75, 1 contradiction)
            Tuple.Create(true, 1.0, -1),   // Dataset 9: Pass (score 1.0, no contradictions)
            Tuple.Create(false, 0.67, -1)   // Dataset 10: Fail (score = .50 with two contradictions).
        };

        var contexts = new List<List<string>> {
            new List<string> { "The sun rises in the east.", "The sun is a planet", "The Earth revolves around the Sun." },
            new List<string> { "Cats are mammals.", "Fish live in water.", "Birds can fly." },
            new List<string> { "The capital of France is Paris.", "The Eiffel Tower is in Paris.", "France is in Europe." },
            new List<string> { "Humans need oxygen to survive.", "Plants produce oxygen through photosynthesis.", "Carbon dioxide is a greenhouse gas." },
            new List<string> { "The Great Wall of China is a historical landmark.", "China is the most populous country.", "Mandarin is the most spoken language in China." },
            new List<string> { "The Amazon rainforest is the largest rainforest.", "It is located in South America.", "The Amazon River flows through it." },
            new List<string> { "The Moon orbits the Earth.", "The Moon has phases like full moon and new moon.", "The Moon has no atmosphere." },
            new List<string> { "Electric cars are powered by batteries.", "They produce no tailpipe emissions.", "Charging stations are needed for electric cars." },
            new List<string> { "The Pacific Ocean is the largest ocean.", "It covers more than 30% of the Earth's surface.", "It is deeper than the Atlantic Ocean." },
            new List<string> { "Mount Everest is the highest mountain.", "It is located in the Himalayas.", "It is over 8,800 meters tall." }
        };

        var inputs = new List<string> {
            "Describe the natural phenomena related to the sun.",
            "Explain the characteristics of different animal groups.",
            "Provide details about France and its landmarks.",
            "Discuss the importance of oxygen for living beings.",
            "Share facts about China and its cultural significance.",
            "Talk about the Amazon rainforest and its features.",
            "Explain the relationship between the Earth and the Moon.",
            "Discuss the benefits and requirements of electric cars.",
            "Provide information about the Pacific Ocean.",
            "Describe Mount Everest and its geographical importance."
        };

        var outputs = new List<string> {
            // Pass (0 contradictions)
            "The sun rises in the east. The sun is a planet. The Earth revolves around the Sun.",
    
            // Fail (2 contradictions, score = 0.5)
            "Cats are mammals. Fish live in water. Birds can fly. Dogs lay eggs.",
    
            // Pass (0 contradictions)
            "The capital of France is Paris. The Eiffel Tower is in Paris. France is in Europe.",
    
            // Fail (1 contradiction, score = 0.75)
            "Humans need oxygen to survive. Plants produce oxygen through photosynthesis. Carbon dioxide is a greenhouse gas. Humans can survive without oxygen.",
    
            // Pass (0 contradictions)
            "The Great Wall of China is a historical landmark. China is the most populous country. Mandarin is the most spoken language in China.",
    
            // Fail (2 contradictions, score = 0.5)
            "The Amazon rainforest is the largest rainforest. It is located in South America. The Amazon River flows through it. The Amazon rainforest is in Africa.",
    
            // Pass (0 contradictions)
            "The Moon orbits the Earth. The Moon has phases like full moon and new moon. The Moon has no atmosphere.",
    
            // Fail (1 contradiction, score = 0.75)
            "Electric cars are powered by batteries. They produce no tailpipe emissions. Charging stations are needed for electric cars. Electric cars run on gasoline.",
    
            // Pass (0 contradictions)
            "The Pacific Ocean is the largest ocean. It covers more than 30% of the Earth's surface. It is deeper than the Atlantic Ocean.",
    
            // Fail (2 contradictions, score = 0.5)
            "Mount Everest is the highest mountain. It is located in the Himalayas. It is over 8,800 meters tall. Mount Everest is underwater."
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

            var check = new CheckConfiguration(outputs[j]);

            var checks = new List<ICheck>();
            for (var i = 0; i < 10; i++)
            {
                checks.Add(new HallucinationCheck(inputs[j], contexts[j], llmCaller));
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
            ExcelExport.Create(Helpers.ExtractResults(results), expected, type.ToString(), "HALLUCINATION");
        }
    }
}