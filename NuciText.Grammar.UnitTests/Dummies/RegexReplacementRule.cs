using NuciText.Grammar.Rules;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class RegexReplacementRule(params RegexReplacement[] replacements) : PatternReplacementRuleBase
    {
        public override string Id => "regex-replacement";

        public override string Description => "Applies raw regular expression replacements.";

        protected override string DoApply(string text) => ApplyRegexReplacements(text, replacements);
    }
}