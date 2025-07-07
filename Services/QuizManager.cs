
using Spectre.Console;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
public static class QuizManager
{
    public static string? selectedTitle { get; set; } = selectedTitle;
    public static string GetPath { get; set; } = "C#_Basics.json";
    public static void Run()
    {
        AnsiConsole.Write(new Rule("[bold yellow]MANAGE QUIZZES[/]")
            .Centered()
            .RuleStyle("grey"));

        var path = GetPath;
        var jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        List<Dictionary<string, object>> parsedQuizs;

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            parsedQuizs = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stream, jsonOptions)!;
        }

        var titles = new List<string>();
        foreach (var item in parsedQuizs)
        {
            var isTrue = ((JsonElement)item["IsActive"]).GetBoolean();
            var title = item["Title"].ToString();
            titles.Add($"{title} {(isTrue ? "(Active)" : "(Disabled)")}");
        }

        titles.Add("Back");

        Action(parsedQuizs, titles);
    }

    public static void Action(List<Dictionary<string, object>> parsedStudents, List<string> titles)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a quiz to manage:")
                .HighlightStyle("yellow") 
                .AddChoices(titles));

        if (selected == "Back")
        {
            QuizBuilder.Run();
            return;
        }
        var selectedTitle = selected.Split(" (")[0];


        var quiz = parsedStudents.First(q =>
            q["Title"]!.ToString() == selectedTitle);

        var isActive = ((JsonElement)quiz["IsActive"]).GetBoolean();


        QuizActions.Run(selectedTitle, isActive);
    }

}