using Spectre;
using Spectre.Console;

public static class InputHelper
{
    public static T AskText<T>(string prompt, IEnumerable<T> options) where T : notnull
    {
        return AnsiConsole.Prompt(new SelectionPrompt<T>()
                .Title(Markup.Escape(prompt))
                .HighlightStyle("yellow")
                .PageSize(10)
                .AddChoices(options));
    }

    public static T AskText<T>(IEnumerable<T> options) where T : notnull
    {
        return AnsiConsole.Prompt(new SelectionPrompt<T>()
                .HighlightStyle("yellow")
                .PageSize(10)
                .AddChoices(options));
    }

    public static T AskText<T>(List<T> options) where T : notnull
    {
        return AnsiConsole.Prompt(new SelectionPrompt<T>()
                .HighlightStyle("yellow")
                .PageSize(10)
                .AddChoices(options));
    }

    public static string AskText(string prompt)
    {
        AnsiConsole.MarkupLine($"[bold]{Markup.Escape(prompt)}[/]");
        return AnsiConsole.Prompt(
            new TextPrompt<string>(">")
                .PromptStyle("yellow")
                .AllowEmpty());
    }

    public static int? AskInt(string prompt)
    {
        AnsiConsole.MarkupLine($"[bold]{Markup.Escape(prompt)}[/]");

        string input = AnsiConsole.Prompt(
            new TextPrompt<string>(">")
                .PromptStyle("yellow")
                .AllowEmpty());

        if (int.TryParse(input, out int value))
            return value;
        else
            return null;
    }

    public static Table BuildTable(IEnumerable<(string, string, string)> rows, string col1 = "#", string col2 = "Type", string col3 = "Prompt")
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(col1)
            .AddColumn(col2)
            .AddColumn(col3);

        foreach (var (first, second, third) in rows)
        {
            table.AddRow(first, second, third);
        }

        return table;
    }

    public static bool AskConfirmation(string prompt)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"[bold]{Markup.Escape(prompt)}[/] [grey](y/n)[/]")
                .PromptStyle("yellow")
                .AllowEmpty());

        var normalized = input.Trim().ToLower();

        if (normalized == "y" || normalized == "yes")
        {
            AnsiConsole.MarkupLine("[green]Quiz saved![/]");
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Quiz not saved.[/]");
            return false;
        }
    }

    public static string AskText(List<Dictionary<string, object>> optionsList)
    {
        var options = optionsList.First(); // yoki kerakli indeks
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Tanlang:[/]")
                .HighlightStyle("yellow")
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(key => $"{key} - {options[key]}"));
    }


}




