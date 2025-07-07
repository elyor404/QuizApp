
using Spectre.Console;

public static class QuizBuilder
{
    public static void Run()
    {
        AnsiConsole.Write(
            new Rule("[bold yellow]TEACHER MODE[/]")
                .Centered()
                .RuleStyle("grey"));

        var option = InputHelper.AskText(new[] { "Create quiz", "Manage quizzes", "Exit" });

        switch (option)
        {
            case "Create quiz": CreateQuiz(); break;
            case "Manage quizzes": QuizManager.Run(); break;
            case "Exit": return;
        }
    }

    public static void CreateQuiz()
    {
        var quizTitle = InputHelper.AskText("[ Title of new quiz ]");
        var quizDescription = InputHelper.AskText("[ Short description ]");
        bool IsActive = true;
        var questions = new List<Dictionary<string, object>>();
        var quiz = new Dictionary<string, object>()
        {
            ["Title"] = quizTitle,
            ["Description"] = quizDescription,
            ["IsActive"] = IsActive,
            ["Questions"] = questions
        };
        while (true)
        {
            var quizType = InputHelper.AskText($"Add questions to “{quizTitle}”", ["Add MCQ", "Add True/False", "Add Short Answer", "Finish Quiz"]);

            switch (quizType)
            {
                case "Add MCQ":
                    questions.Add(McqQuestion.Run());
                    break;
                case "Add True/False":
                    questions.Add(TrueFalseQuestion.Run());
                    break;
                case "Add Short Answer":
                    questions.Add(ShortAnswerQuestion.Run());
                    break;
                case "Finish Quiz":
                    QuizStorage.ViewSummary(questions);
                    var saveOption = InputHelper.AskConfirmation("Save quiz? [y/n]");

                    if (saveOption)
                    {
                        QuizStorage.SaveQuizAsJson(quiz);
                    }
                    return;
            }
        }
    }

}
