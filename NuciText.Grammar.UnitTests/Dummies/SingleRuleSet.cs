using System.Collections.Generic;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class SingleRuleSet(IGrammarRule rule) : GrammarRuleSet
    {
        public override string LanguageCode => "en";

        public override IReadOnlyList<IGrammarRule> Rules => [rule];
    }
}