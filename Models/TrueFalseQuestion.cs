public static class TrueFalseQuestion
{
    public static Dictionary<string, object> Run()
    {
        var question = InputHelper.AskText("Question prompt");

        var correctOptionKey = InputHelper.AskText("Correct option key", ["True", "False"]);

        var trueFalse = new Dictionary<string, object>
        {
            ["Type"] = "True/False",
            ["Prompt"] = question,
            ["Answer"] = correctOptionKey
        };
        return trueFalse;
    }

}
