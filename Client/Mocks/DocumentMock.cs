using MindCheck.AiSearch;
using MindCheck.Checks;
using MindCheck.Checks.LLM;
using MindCheck.Interfaces;

namespace Client.Mocks;

public static class DocumentMock
{
    public static void Run(OutputType outputType, AiSearchType aiSearchType)
    {

var mockedResponses = new List<string> {
    "Social media has revolutionized communication, enabling instant connections across continents through platforms like Facebook and TikTok. While it facilitates rapid news dissemination during crises, it also spreads misinformation and raises privacy concerns. Despite its challenges, social media amplifies marginalized voices and fosters advocacy for societal change.",
    "Social media plays a pivotal role in cultural and political movements such as #MeToo and Black Lives Matter, uniting millions under shared causes. However, echo chambers fueled by algorithms deepen societal divisions and prioritize sensationalism over accuracy. Policymakers face the challenge of regulating these platforms without compromising free speech or innovation.",
    "The solar system is a complex structure centered around the Sun, whose gravity orchestrates the movement of planets, moons, and asteroids. Earth stands out as the only known planet to harbor life, while Mars intrigues scientists with evidence of ancient rivers. Beyond Neptune lies the Kuiper Belt, home to icy bodies like Pluto.",
    "The solar system formed 4.6 billion years ago from a collapsing molecular cloud that created a spinning disk of gas and dust. Inner rocky planets emerged closer to the Sun, while outer gas giants retained their compositions farther away. Modern missions like Perseverance explore Mars for signs of life, expanding our understanding of cosmic origins.",
    "Social media has transformed communication globally while empowering movements like #MeToo and Black Lives Matter through hashtags. However, it also fosters misinformation and echo chambers that hinder constructive dialogue. Balancing its benefits with its risks remains a pressing challenge for society and policymakers alike.",
    "The solar system's formation began 4.6 billion years ago with a collapsing molecular cloud that gave rise to rocky inner planets and gaseous outer giants. Earth harbors life due to its unique atmosphere, while Mars offers clues about ancient rivers and possible microbial life. Beyond Neptune lies the Kuiper Belt, holding remnants of the system's early days.",
    "While social media connects people globally, the solar system demonstrates cosmic interconnectedness through gravitational forces binding celestial bodies. Both systems reveal intricate balances—whether through algorithms or planetary orbits—that shape their dynamics. Studying these phenomena deepens our understanding of human interaction and universal order.",
    "Social media has revolutionized communication, enabling instant connections across continents through platforms like Facebook and TikTok. While it facilitates rapid news dissemination during crises, it also spreads misinformation and raises privacy concerns. Despite its challenges, social media amplifies marginalized voices and fosters advocacy for societal change.",
    "Social media plays a pivotal role in cultural and political movements such as #MeToo and Black Lives Matter, uniting millions under shared causes. However, echo chambers fueled by algorithms deepen societal divisions and prioritize sensationalism over accuracy. Policymakers face the challenge of regulating these platforms without compromising free speech or innovation.",
    "Social media has revolutionized communication, enabling instant connections across continents through platforms like Facebook and TikTok. While it facilitates rapid news dissemination during crises, it also spreads misinformation and raises privacy concerns. Despite its challenges, social media amplifies marginalized voices and fosters advocacy for societal change.",
};
var requiredDocumentsList = new List<List<string>>
{
    new List<string>
    {
    "socialmedia-1_convertedToPDF.pdf"
    },
    new List<string>
    {
    "socialmedia-2_convertedToPDF.pdf"
    },
    new List<string>
    {
    "solarsystem-1_convertedToPDF.pdf"
    },
    new List<string>
    {
    "solarsystem-2_convertedToPDF.pdf"
    },
    new List<string>
    {
    "socialmedia-1_convertedToPDF.pdf",
    "socialmedia-2_convertedToPDF.pdf"
    },
    new List<string>
    {
    "solarsystem-1_convertedToPDF.pdf",
    "solarsystem-2_convertedToPDF.pdf"
    },
    new List<string>
    {
    "socialmedia-1_convertedToPDF.pdf",
    "solarsystem-1_convertedToPDF.pdf"
    },
    new List<string>
    {
        "economics.pdf"
    },
    new List<string>
    {
        "finances.pdf",
    },
    new List<string>
    {
        "solarsystem-1_convertedToPDF.pdf"
    },
};

var expectedResults = new List<Tuple<bool, double, int>> {
    Tuple.Create(true, 1.0, -1), 
    Tuple.Create(true, 1.0, -1), 
    Tuple.Create(true, 1.0, -1),  
    Tuple.Create(true, 1.0, -1),  
    Tuple.Create(true, 1.0, -1), 
    Tuple.Create(true, 1.0, -1), 
    Tuple.Create(true, 1.0, -1),  
    Tuple.Create(false, 0.0, -1),
    Tuple.Create(false, 0.0, -1), 
    Tuple.Create(false, 0.0, -1)  
};
        var results = new List<IResult>();
        for (var j = 0; j < 10; j++)
        {

            var check = new CheckConfiguration(mockedResponses[j]);

            var checks = new List<ICheck>();
            for (var i = 0; i < 10; i++)
            {
                checks.Add(new DocumentCheck(aiSearchType, requiredDocumentsList[j]));
            }

            checks.ForEach(c => check.Then(c));
            results.AddRange(check.Execute());
        }

        var data = Helpers.ExtractResults(results);
        if (outputType == OutputType.Console)
        {
            Helpers.PrintResults(data, aiSearchType.ToString());
        }
        else
        {
            ExcelExport.Create(Helpers.ExtractResults(results), expectedResults, aiSearchType.ToString(), "DOCUMENT");
        }
    }
}