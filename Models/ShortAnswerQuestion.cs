public static class ShortAnswerQuestion
{
    public static Dictionary<string, object> Run()
    {
        var question = InputHelper.AskText("Question prompt");

        var correctOptionKey = InputHelper.AskText("[ Correct option key ]");

        int? timeLimit = InputHelper.AskInt("[ Time limit in seconds (ENTER for none) ]");

        var mcq = new Dictionary<string, object>
        {
            ["Type"] = "ShortAnswer",
            ["Prompt"] = question,
            ["Answer"] = correctOptionKey

        };
        if (timeLimit.HasValue)
        {
            mcq["TimeLimitSeconds"] = timeLimit;
        }
        return mcq;
    }
}


    //   "Type": "ShortAnswer",
    //   "Prompt": "What keyword defines a constant?",
    //   "CorrectAnswer": "const",
    //   "TimeLimitSeconds": 15
    // }