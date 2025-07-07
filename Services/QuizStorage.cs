using Spectre.Console;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public static class QuizStorage
{
    public static void ViewSummary(List<Dictionary<string, object>> questions)
    {
        AnsiConsole.MarkupLine("\n[bold]All questions entered. Summary:[/]\n");

        var rows = new List<(string, string, string)>();

        for (int i = 0; i < questions.Count; i++)
        {
            var q = questions[i];
            var type = q["Type"].ToString();
            var prompt = q["Prompt"].ToString();

            if (q.TryGetValue("TimeLimitSeconds", out var seconds))
            {
                type += $" ({seconds}s)";
            }

            rows.Add(((i + 1).ToString(), type ?? "", prompt ?? ""));
        }

        var table = InputHelper.BuildTable(rows);
        AnsiConsole.Write(table);
    }

    public static void SaveQuizAsJson(Dictionary<string, object> quiz)
    {
        var path = "C#_Basics.json";
        var jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
    List<Dictionary<string, object>> existingData;


    if (File.Exists(path) && !string.IsNullOrWhiteSpace(File.ReadAllText(path)))
    {
        try
        {
            existingData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(File.ReadAllText(path), jsonOptions)!;
        }
        catch (JsonException)
        {
            existingData = new List<Dictionary<string, object>>();
        }
    }
    else
    {
        existingData = new List<Dictionary<string, object>>();
    }

    existingData.Add(quiz);

        var jsonStudents = JsonSerializer.Serialize(existingData, jsonOptions);
        using (var filestream = new StreamWriter(path))
        {
            filestream.Write(jsonStudents);
        }
        ;
         AnsiConsole.MarkupLine("[bold]Writing file “C# Basics.json” … done.[/]");
        AnsiConsole.MarkupLine("[green]Quiz created successfully![/]");
    }
}


