using NuciText.Grammar.Rules;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class WordReplacementRule(params RegexReplacement[] replacements) : PatternReplacementRuleBase
    {
        public override string Id => "word-replacement";

        public override string Description => "Applies whole-word replacements.";

        protected override string DoApply(string text) => ApplyWordReplacements(text, replacements);
    }
}