using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public class GrammarCorrectorTests
    {
        sealed class DoubleSpaceRule : GrammarRule
        {
            public override string Id => "double-space";
            public override string Description => "Replaces double spaces with a single space.";
            protected override string DoApply(string text) => text.Replace("  ", " ");
        }

        sealed class EmptyRuleSet : GrammarRuleSet
        {
            public override string LanguageCode => "en";
            public override IReadOnlyList<IGrammarRule> Rules => Array.Empty<IGrammarRule>();
        }

        sealed class SingleRuleSet(IGrammarRule rule) : GrammarRuleSet
        {
            readonly IGrammarRule rule = rule;

            public override string LanguageCode => "en";
            public override IReadOnlyList<IGrammarRule> Rules => new[] { rule };
        }

        sealed class MultiRuleSet(params IGrammarRule[] rules) : GrammarRuleSet
        {
            readonly IGrammarRule[] rules = rules;

            public override string LanguageCode => "en";
            public override IReadOnlyList<IGrammarRule> Rules => rules;
        }

        sealed class UppercaseFirstCharRule : GrammarRule
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

        [Test]
        public void GivenNullRuleSet_WhenConstructing_ThenThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new GrammarCorrector(null!));

        [Test]
        public void GivenEmptyRuleSet_WhenCorrectingNullText_ThenThrowsArgumentNullException()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            Assert.Throws<ArgumentNullException>(() => corrector.Correct(null!));
        }

        [Test]
        public void GivenEmptyRuleSet_WhenCorrectingText_ThenReturnsSameText()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("hello  world"));
        }

        [Test]
        public void GivenMatchingRule_WhenCorrectingText_ThenAppliesCorrection()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("hello world"));
        }

        [Test]
        public void GivenNonMatchingRule_WhenCorrectingText_ThenReturnsSameText()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello world");

            Assert.That(result, Is.EqualTo("hello world"));
        }

        [Test]
        public void GivenMultipleMatchingRules_WhenCorrectingText_ThenAppliesAllInOrder()
        {
            var ruleSet = new MultiRuleSet(
                new DoubleSpaceRule(),
                new UppercaseFirstCharRule());

            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("Hello world"));
        }
    }
}
