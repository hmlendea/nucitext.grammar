namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class TrimRule : GrammarRule
    {
        public override string Id => "trim";

        public override string Description => "Trims leading and trailing whitespace.";

        protected override string DoApply(string text) => text.Trim();
    }
}