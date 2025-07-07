using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;

public static class QuizRunner
{
    public static List<Dictionary<string, object>>? DataTure { get; set; }

    public static void Run()
    {
        AnsiConsole.Write(new Rule("[bold yellow]Student Mode[/]")
            .Centered()
            .RuleStyle("grey"));

        var path = QuizManager.GetPath;

        var options = new JsonSerializerOptions()
        {
            WriteIndented=true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        List<Dictionary<string, object>>? data;
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stream, options);
        }

        if (data?.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No quizzes available yet[/]");
            return;
        }

        DataTure = new List<Dictionary<string, object>>();

        foreach (var item in data!)
        {
            var isTrue = ((JsonElement)item["IsActive"]).GetBoolean();
            if (isTrue)
            {
                DataTure.Add(item);
            }
        }
        if (DataTure.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No active quizzes found :([/]");
            AnsiConsole.MarkupLine("[bold red]Ask your teacher to enable at least one quiz.[/]");
        }
        else
        {
            QuizMeta.Run();
        }
    }
}
