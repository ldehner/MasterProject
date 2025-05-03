using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public static class KeywordMock
{
    public static void Run(OutputType outputType)
    {
        var keywordsList = new List<List<string>> {
    new List<string> { "teamwork", "collaboration", "creativity" },
    new List<string> { "culture", "diversity", "heritage" },
    new List<string> { "kindness", "relationships", "positivity" },
    new List<string> { "opinions", "perspectives", "understanding" },
    new List<string> { "education", "empowerment", "equality" },
    new List<string> { "gender", "equality", "rights" },
    new List<string> { "mental health", "awareness", "support" },
    new List<string> { "poverty", "causes", "solutions" },
    new List<string> { "technology", "communication", "innovation" },
    new List<string> { "immigration", "opportunities", "integration" }
};

var mockedResponses = new List<string> {
    // Pass (All keywords included)
    "Teamwork fosters collaboration and creativity among team members.",
    
    // Pass (All keywords included)
    "Culture and diversity are essential for preserving heritage.",
    
    // Pass (All keywords included)
    "Kindness builds strong relationships and promotes positivity.",
    
    // Pass (All keywords included)
    "People with different opinions bring unique perspectives and understanding.",
    
    // Fail (Missing 'empowerment')
    "Education promotes equality and helps individuals grow.",
    
    // Fail (Missing 'rights')
    "Gender equality is important for society.",
    
    // Pass (All keywords included)
    "Mental health awareness provides support to those in need.",
    
    // Fail (Missing 'solutions')
    "Poverty has many causes but can be reduced through effort.",
    
    // Pass (All keywords included)
    "Technology drives communication and fosters innovation.",
    
    // Fail (Missing 'integration')
    "Immigration creates opportunities for economic growth."
};

var expectedResults = new List<Tuple<bool, double, int>> {
    Tuple.Create(true, 1.0, -1),   // Dataset 1: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(true, 1.0, -1),   // Dataset 2: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(true, 1.0, -1),   // Dataset 3: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(true, 1.0, -1),   // Dataset 4: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(false, 0.6666666666666666, -1), // Dataset 5: Fail (~0.67, missing 'empowerment')
    Tuple.Create(false, 0.6666666666666666, -1), // Dataset 6: Fail (~0.67, missing 'rights')
    Tuple.Create(true, 1.0, -1),   // Dataset 7: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(false, 0.6666666666666666, -1), // Dataset 8: Fail (~0.67, missing 'solutions')
    Tuple.Create(true, 1.0, -1),   // Dataset 9: Pass (score 1.0, all 3 keywords included)
    Tuple.Create(false, 0.6666666666666666, -1)  // Dataset 10: Fail (~0.67, missing 'integration')
};
            
        
        var results = new List<IResult>();
        for (var j = 0; j < 10; j++)
        {

            var check = new CheckConfiguration(mockedResponses[j]);

            var checks = new List<ICheck>();
            for (var i = 0; i < 10; i++)
            {
                checks.Add(new KeyWordCheck(keywordsList[j]));
            }

            checks.ForEach(c => check.Then(c));
            results.AddRange(check.Execute());
        }

        var data = Helpers.ExtractResults(results);
        if (outputType == OutputType.Console)
        {
            Helpers.PrintResults(data, outputType.ToString());
        }
        else
        {
            ExcelExport.Create(Helpers.ExtractResults(results), expectedResults, "", "KEYWORD");
        }
    }
}