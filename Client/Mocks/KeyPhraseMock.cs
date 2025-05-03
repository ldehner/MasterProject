using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public static class KeyPhraseMock
{
    public static void Run(OutputType outputType)
    {
var keyphrasesList = new List<List<string>> {
    new List<string> { "fosters collaboration", "encourages creativity", "teamwork benefits" },
    new List<string> { "preserving heritage", "importance of diversity", "cultural exchange" },
    new List<string> { "builds strong relationships", "promotes positivity", "acts of kindness" },
    new List<string> { "different perspectives", "understanding opinions", "value of discussion" },
    new List<string> { "education empowers individuals", "promotes equality", "knowledge is power" },
    new List<string> { "gender equality matters", "equal rights for all", "breaking stereotypes" },
    new List<string> { "mental health awareness", "providing support", "reducing stigma" },
    new List<string> { "causes of poverty", "solutions to poverty", "addressing inequality" },
    new List<string> { "advances in technology", "innovation drives progress", "enhancing communication" },
    new List<string> { "immigration creates opportunities", "integration into society", "economic contributions" }
};

var mockedResponses = new List<string> {
    // Pass (All keyphrases included)
    "Teamwork fosters collaboration and encourages creativity. Teamwork benefits everyone involved.",
    
    // Pass (All keyphrases included)
    "Preserving heritage highlights the importance of diversity. Cultural exchange enriches societies.",
    
    // Pass (All keyphrases included)
    "Acts of kindness builds strong relationships and promotes positivity.",
    
    // Pass (All keyphrases included)
    "Different perspectives help in understanding opinions and show the value of discussion.",
    
    // Fail (Missing exact match for 'knowledge is power')
    "Education empowers individuals and promotes equality, but knowledge gives strength.",
    
    // Fail (Missing exact match for 'breaking stereotypes')
    "Gender equality matters and ensures equal rights for everyone in society.",
    
    // Pass (All keyphrases included)
    "Mental health awareness is essential for reducing stigma and providing support to those in need.",
    
    // Fail (Missing exact match for 'addressing inequality')
    "The causes of poverty are complex, but solutions to poverty require addressing social gaps.",
    
    // Pass (All keyphrases included)
    "Advances in technology enhancing communication and innovation drives progress.",
    
    // Fail (Missing exact match for 'economic contributions')
    "Immigration creates opportunities and promotes integration into society, benefiting the economy."
};

var expectedResults = new List<Tuple<bool, double, int>> {
    Tuple.Create(true, 1.0, 3),   // Dataset 1: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(true, 1.0, 3),   // Dataset 2: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(true, 1.0, 3),   // Dataset 3: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(true, 1.0, 3),   // Dataset 4: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(false, 0.6666666666666666, 3), // Dataset 5: Fail (~0.67, missing exact match for 'knowledge is power')
    Tuple.Create(false, 0.3333333333333333, 3), // Dataset 6: Fail (~0.67, missing exact match for 'breaking stereotypes')
    Tuple.Create(true, 1.0, 3),   // Dataset 7: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(false, 0.6666666666666666, 3), // Dataset 8: Fail (~0.67, missing exact match for 'addressing inequality')
    Tuple.Create(true, 1.0, 3),   // Dataset 9: Pass (score 1.0, all 3 keyphrases included)
    Tuple.Create(false, 0.6666666666666666, 3)  // Dataset 10: Fail (~0.67, missing exact match for 'economic contributions')
};

            
        
        var results = new List<IResult>();
        for (var j = 0; j < 10; j++)
        {

            var check = new CheckConfiguration(mockedResponses[j]);

            var checks = new List<ICheck>();
            for (var i = 0; i < 10; i++)
            {
                checks.Add(new KeyWordCheck(keyphrasesList[j]));
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
            ExcelExport.Create(Helpers.ExtractResults(results), expectedResults, "", "KEYPHRASE");
        }
    }
}