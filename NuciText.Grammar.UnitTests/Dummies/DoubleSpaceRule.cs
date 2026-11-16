namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class DoubleSpaceRule : GrammarRule
    {
        public override string Id => "double-space";

        public override string Description => "Replaces double spaces with a single space.";

        protected override string DoApply(string text) => text.Replace("  ", " ");
    }
}