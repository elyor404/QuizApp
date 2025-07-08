using Spectre.Console;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.VisualBasic;

public static class QuizActions
{
    public static string TitleName { get; set; } = "";
    public static void Run(string titleName, bool isActiveValue)
    {
        TitleName = titleName;
        var statusAction = isActiveValue ? "Disable Quiz" : "Activate Quiz";

        var insideOfManage = InputHelper.AskText("Actions:", [statusAction, "Delete Quiz", "View Summary", "Back"]);

        switch (insideOfManage)
        {
            case "Disable Quiz":
            case "Activate Quiz":
                Activate(titleName);
                break;
            case "Delete Quiz":
                Delete();
                break;
            case "View Summary":
                ViewSummary();
                break;
            case "Back":
                QuizManager.Run();
                break;
        }
    }

    public static void Activate(string titlePath)
    {
        string path = "C#_Basics.json";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        List<Dictionary<string, object>> quizzes;

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            quizzes = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stream, options)!;
        }

        string? valueIsTrue = null;

        foreach (var item in quizzes)
        {
            if (item["Title"].ToString() == titlePath)
            {
                if (item.TryGetValue("IsActive", out var value))
                {
                    var isActive = ((JsonElement)value).GetBoolean();
                    item["IsActive"] = !isActive;
                    valueIsTrue = !isActive is true ? "Quiz Enabled" : "Quiz Disabled";
                }
                else
                {
                    item["IsActive"] = false;
                    valueIsTrue = "Quiz Disabled";
                }
            }
        }

        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            JsonSerializer.Serialize(stream, quizzes, options);
        }
        AnsiConsole.MarkupInterpolated($"[green]{valueIsTrue}[/]");
    }

    public static void Delete()
    {
        AnsiConsole.MarkupLine($"[red]Danger! This will permanently delete “{TitleName}”.[/]");
        var button = InputHelper.AskText("Type DELETE to confirm → ").Trim().ToLower();

        if (button == "delete")
        {
            var path = QuizManager.Path;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            List<Dictionary<string, object>> parsedInfo;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                parsedInfo = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stream, options)!;
            }

            int before = parsedInfo.Count;

            parsedInfo.RemoveAll(item =>
                item.ContainsKey("Title") &&
                item["Title"] is JsonElement titleElement &&
                titleElement.GetString() == TitleName);

            int after = parsedInfo.Count;

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(stream, parsedInfo, options);
            }

            if (after < before)
                AnsiConsole.MarkupLine($"[red]Quiz \"{TitleName}\" deleted successfully.[/]");
            else
                AnsiConsole.MarkupLine($"[yellow]No quiz named \"{TitleName}\" was not found. Nothing deleted.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Incorrect confirmation string. Delete aborted.[/]");
        }
    }



    public static void ViewSummary()
    {
        var path = QuizManager.Path;
        var data = new List<Dictionary<string, object>>();

        var options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stream, options);
        }

        foreach (var item in data!)
        {
            if (item["Title"]!.ToString() == TitleName)
            {
                var questionsElement = (JsonElement)item["Questions"];

                var questions = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(questionsElement.GetRawText());

                if (questions is not null)
                    QuizStorage.ViewSummary(questions);
            }
        }
    }
}