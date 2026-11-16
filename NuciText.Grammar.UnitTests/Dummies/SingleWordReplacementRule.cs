using NuciText.Grammar.Rules;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class SingleWordReplacementRule(string pattern, string replacement) : PatternReplacementRuleBase
    {
        public override string Id => "single-word-replacement";

        public override string Description => "Replaces a single whole word.";

        protected override string DoApply(string text) => ReplaceWord(text, pattern, replacement);
    }
}