using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public static class ContextMock
{
    public static void Run(LlmType type, OutputType outputType)
    {
        var expected = new List<Tuple<bool, double, int>>
        {
            Tuple.Create(true, 1.0, 3), // Dataset 1 (Pass: All statements are relevant)
            Tuple.Create(false, 0.5, 4), // Dataset 2 (Fail: 2/4 relevant statements)
            Tuple.Create(true, 1.0, 3), // Dataset 3 (Pass: All statements are relevant)
            Tuple.Create(false, 0.5, 4), // Dataset 4 (Fail: 2/4 relevant statements)
            Tuple.Create(true, 1.0, 3), // Dataset 5 (Pass: All statements are relevant)
            Tuple.Create(false, 0.5, 4), // Dataset 6 (Fail: 2/4 relevant statements)
            Tuple.Create(true, 1.0, 2), // Dataset 7 (Pass: All statements are relevant)
            Tuple.Create(false, 0.5, 4), // Dataset 8 (Fail: 2/4 relevant statements)
            Tuple.Create(true, 0.75, 4), // Dataset 9 (Pass: 3/4 relevant statements)
            Tuple.Create(true, 0.75, 4), // Dataset 10 (Pass: 3/4 relevant statements)

        };
        var inputPrompts = new List<string>
        {
            "Explain the benefits of regular exercise.",
            "Explain the benefits of regular exercise.",
            "What are the causes of climate change?",
            "What are the causes of climate change?",
            "Describe the process of photosynthesis.",
            "Describe the process of photosynthesis.",
            "What are the advantages of remote work?",
            "What are the advantages of remote work?",
            "What are the advantages of electric vehicles?",
            "Describe the importance of renewable energy."
        };
        var expectedStatements = new List<int>()
        {
            3, // Dataset 1 (Pass: All statements are relevant)
            4, // Dataset 2 (Fail: 2/4 relevant statements)
            3, // Dataset 3 (Pass: All statements are relevant)
            4, // Dataset 4 (Fail: 2/4 relevant statements)
            3, // Dataset 5 (Pass: All statements are relevant)
            4, // Dataset 6 (Fail: 2/4 relevant statements)
            2, // Dataset 7 (Pass: All statements are relevant)
            4, // Dataset 8 (Fail: 2/4 relevant statements)
            4, // Dataset 9 (Pass: 3/4 relevant statements)
            4, // Dataset 10 (Pass: 3/4 relevant statements)
        };
        var mockedResponses = new List<string>
        {
            // Pass
            "Regular exercise improves cardiovascular health. It boosts mental well-being. Exercise helps in maintaining a healthy weight.",

            // Fail (Mixed: relevant + irrelevant)
            "Regular exercise improves cardiovascular health. Traveling is a great way to explore new cultures. Exercise helps in maintaining a healthy weight. Cooking recipes can be found online.",

            // Pass
            "Climate change is caused by greenhouse gas emissions. Deforestation contributes to global warming. Burning fossil fuels releases carbon dioxide into the atmosphere.",

            // Fail (Mixed: relevant + irrelevant)
            "Climate change is caused by greenhouse gas emissions. The history of ancient civilizations is fascinating. Deforestation contributes to global warming. Music can evoke strong emotions.",

            // Pass
            "Photosynthesis converts sunlight into energy. Plants use chlorophyll for this process. Oxygen is released as a byproduct.",

            // Fail (Mixed: relevant + irrelevant)
            "Photosynthesis converts sunlight into energy. The weather forecast predicts rain tomorrow. Plants use chlorophyll for this process. Movies are a popular form of entertainment.",

            // Pass
            "Remote work allows flexibility in schedules. It reduces commuting time and costs.",

            // Fail (Mixed: relevant + irrelevant)
            "Remote work allows flexibility in schedules. The Eiffel Tower is located in Paris. It reduces commuting time and costs. Birds migrate during certain seasons.",

            // Pass ( 3 / 4 relevant)
            "Electric vehicles reduce greenhouse gas emissions. They are more energy-efficient compared to traditional cars. EVs require less maintenance due to fewer moving parts. Watching movies is a popular leisure activity.",

            // Pass ( 3 / 4 relevant)
            "Renewable energy reduces dependence on fossil fuels. It helps combat climate change by lowering carbon emissions. Renewable sources like solar and wind are sustainable. Playing video games can improve hand-eye coordination."
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
                checks.Add(new ContextualRelevancyCheck(inputPrompts[j], llmCaller, statementCount: expectedStatements[j]));
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
            ExcelExport.Create(Helpers.ExtractResults(results), expected, type.ToString(), "CONTEXT");
        }
    }
}