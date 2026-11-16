using System.Collections.Generic;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class MultiRuleSet(params IGrammarRule[] rules) : GrammarRuleSet
    {
        public override string LanguageCode => "en";

        public override IReadOnlyList<IGrammarRule> Rules => rules;
    }
}