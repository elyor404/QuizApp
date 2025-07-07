using System.Reflection.Emit;
using Spectre.Console;

public static class McqQuestion
{
    public static Dictionary<string, object> Run()
    {
        var question = InputHelper.AskText("Question prompt");

        int? countOfOptions = InputHelper.AskInt("How many options would be? [ count must be integer]");


        var optionsDictioanry = new Dictionary<string, string> { };


        char i = 'A';
        for (var _ = 0; _ < countOfOptions; _++)
        {
            optionsDictioanry[i.ToString()] = InputHelper.AskText($"Option {i}");
            i++;
        }

        var correctOptionKey = InputHelper.AskText("Correct option key", optionsDictioanry.Keys.ToArray());

        int? timeLimit = InputHelper.AskInt("Time limit in seconds (ENTER for none)");

        var mcq = new Dictionary<string, object>
        {
            ["Type"] = "MCQ",
            ["Prompt"] = question,
            ["Options"] = optionsDictioanry,
            ["Answer"] = correctOptionKey
        };

        if (timeLimit.HasValue)
        {
            mcq["TimeLimitSeconds"] = timeLimit.Value;
        }

        AnsiConsole.MarkupLine($"[green]Question added to MCQ successfully[/]");
        return mcq;
    }
}
