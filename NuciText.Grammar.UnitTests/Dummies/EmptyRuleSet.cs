using System.Collections.Generic;

namespace NuciText.Grammar.UnitTests.Dummies
{
    internal sealed class EmptyRuleSet : GrammarRuleSet
    {
        public override string LanguageCode => "en";

        public override IReadOnlyList<IGrammarRule> Rules => [];
    }
}