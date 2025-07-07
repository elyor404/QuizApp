using Spectre.Console;

AnsiConsole.Write(new Rule("[bold yellow]QUIZ APP[/]").Centered().RuleStyle("grey"));

var choice = InputHelper.AskText(
    "Select application mode:",
    ["Teacher Mode", "Student Mode", "Exit"]);

switch (choice)
{
    case "Teacher Mode": QuizBuilder.Run(); break;
    case "Student Mode": QuizRunner.Run(); break;
    case "Exit":
        AnsiConsole.Markup("[green]Goodbye[/]");
        return;
}

