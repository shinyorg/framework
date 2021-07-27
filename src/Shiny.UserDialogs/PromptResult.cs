namespace Shiny.UserDialogs
{
    public enum PromptChoice
    {
        Neutral,
        Negative,
        Postive
    }


    public struct PromptResult
    {
        public PromptResult(PromptChoice choice, string? value)
        {
            this.Choice = choice;
            this.Value = value;
        }

        public PromptChoice Choice { get; }
        public string? Value { get; }
    }
}