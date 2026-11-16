namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class FixedApplicabilityRule(bool applicability) : GrammarRule
    {
        public override string Id => "fixed-applicability";

        public override string Description => "Reports a fixed applicability result.";

        protected override bool CheckApplicability(string text) => applicability;

        protected override string DoApply(string text) => text + "!";
    }
}