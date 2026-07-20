namespace NewAI_CV_builder.Utilities.ReversePrompts
{
    public class ReverseHookItem
    {
        public string Name { get; set; } = "";
        public string Hook { get; set; } = "";
        public string Content { get; set; } = "";

        public override string ToString() => Name;
    }
}
