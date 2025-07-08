using Spectre.Console;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

public class QuizMeta
{
    public static double FinalScore { get; set; } = 0;
    public static void Run()
    {
        var data = QuizRunner.DataTure ?? [];
        // var a= data.Select<List<Dictionary<string, object>>>(x => x["Title"].ToString());
        List<string> quiz = [];
        foreach (var item in data)
        {
            quiz.Add(item["Title"].ToString() ?? "");
        }

        var quizResult = InputHelper.AskText("Select a quiz: ", quiz);

        foreach (var item in data)
        {
            if (item["Title"].ToString() == quizResult)
            {
                AnsiConsole.MarkupLine($"[bold white]Quiz name: {item["Title"]}[/]");
                AnsiConsole.MarkupLine($"[bold white]Quiz description: {item["Description"]}[/]");

                AnsiConsole.MarkupLine("[yellow]Press ENTER to begin…[/]");
                var key = AnsiConsole.Console.Input.ReadKey(true);

                if (key.HasValue && key.Value.Key == ConsoleKey.Enter)
                {
                    StartQuiz(item);
                }
            }
        }

    }


    public static void StartQuiz(Dictionary<string, object> typeQuiz)
    {
        var questionsElement = (JsonElement)typeQuiz["Questions"];

        var questions = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(questionsElement.GetRawText()) ?? [];

        int n = 1;
        foreach (var item in questions)
        {
            int count = questions.Count;
            if (item["Type"].ToString() == "MCQ")
            {
                Multiple(item, n, count);
            }
            else if (item["Type"].ToString() == "ShortAnswer")
            {
                ShortAnswer(item, n, count);
            }
            else
            {
                TrueFalse(item, n, count);
            }
            n++;
        }
        AnsiConsole.MarkupLine($"Total: {FinalScore} / {n - 1}  ({FinalScore * 100 / (n - 1):F1}% => {(FinalScore * 100 / (n - 1) >= 70 ? "[green]Passed[/]" : "[red]Failed[/]")})");
    }

    public static void Multiple(Dictionary<string, object> item, int n, int count)
    {
        var stopwatch = Stopwatch.StartNew();


        AnsiConsole.WriteLine($"[ Question {n} / {count}  | ⏱ {item["TimeLimitSeconds"]} s ]");

        var questionsElement = (JsonElement)item["Options"];
        var options = JsonSerializer.Deserialize<Dictionary<string, object>>(questionsElement.GetRawText()) ?? new();

        List<string> lst = [];
        foreach (var narsa in options)
        {
            lst.Add($"{narsa.Key} => {narsa.Value}");
        }

        AnsiConsole.WriteLine($"{item["Prompt"]}");
        var javob = InputHelper.AskText(lst);
        stopwatch.Stop();
        var correctAnswer = javob[0].ToString();

        AnsiConsole.WriteLine($"Answer chosen within {stopwatch.Elapsed.TotalSeconds:F1}s → {correctAnswer}");
        var timeLimitElement = (JsonElement)item["TimeLimitSeconds"];
        double exactTime = timeLimitElement.GetDouble();


        double spentTime = stopwatch.Elapsed.TotalSeconds;
        if (correctAnswer == item["Answer"].ToString() && spentTime < exactTime)
        {
            AnsiConsole.MarkupLine("[green]✔ Correct![/]");
            FinalScore += 1;
        }
        else if (correctAnswer == item["Answer"].ToString() && spentTime > exactTime)
        {
            AnsiConsole.MarkupLine("[yellow]⚠ Time exceeded. Half credit awarded.[/]");
            FinalScore += 0.5 ;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]✖ Incorrect.[/]");
        }
    }


    public static void ShortAnswer(Dictionary<string, object> item, int n, int count)
    {
        var stopwatch = Stopwatch.StartNew();


        AnsiConsole.WriteLine($"[ Question {n} / {count}  | ⏱ {item["TimeLimitSeconds"]} s ]");

        var correctAnswer = InputHelper.AskText(item["Prompt"].ToString() ?? "");
        stopwatch.Stop();


        AnsiConsole.WriteLine($"Answer chosen within {stopwatch.Elapsed.TotalSeconds:F1}s");
        var timeLimitElement = (JsonElement)item["TimeLimitSeconds"];
        double exactTime = timeLimitElement.GetDouble();


        double spentTime = stopwatch.Elapsed.TotalSeconds;
        if (correctAnswer == item["Answer"].ToString() && spentTime < exactTime)
        {
            AnsiConsole.MarkupLine("[green]✔ Correct![/]");
            FinalScore += 1;
        }
        else if (correctAnswer == item["Answer"].ToString() && spentTime > exactTime)
        {
            AnsiConsole.MarkupLine("[yellow]⚠ Time exceeded. Half credit awarded.[/]");
            FinalScore += 0.5 ;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]✖ Incorrect.[/]");
        }
    }
    

    public static void TrueFalse(Dictionary<string, object> item, int n, int count)
    {
        var stopwatch = Stopwatch.StartNew();


        AnsiConsole.WriteLine($"[ Question {n} / {count} ]");

        var correctAnswer = InputHelper.AskText(item["Prompt"].ToString() ?? "",["True","False"]);
        stopwatch.Stop();

        if (correctAnswer == item["Answer"].ToString())
        {
            AnsiConsole.MarkupLine("[green]✔ Correct![/]");
            FinalScore += 1;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]✖ Incorrect.[/]");
        }
    }
}