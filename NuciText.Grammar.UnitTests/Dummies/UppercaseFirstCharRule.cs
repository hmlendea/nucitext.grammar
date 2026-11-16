namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class UppercaseFirstCharRule : GrammarRule
    {
        public override string Id => "uppercase-first-char";

        public override string Description => "Uppercases the first character of the text.";

        protected override string DoApply(string text)
        {
            if (text.Length == 0)
            {
                return text;
            }

            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}