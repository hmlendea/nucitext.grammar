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

        [Test]
        public void Constructor_NullRuleSet_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GrammarCorrector(null!));
        }

        [Test]
        public void Correct_NullText_ThrowsArgumentNullException()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            Assert.Throws<ArgumentNullException>(() => corrector.Correct(null!));
        }

        [Test]
        public void Correct_EmptyRuleSet_ReturnsSameText()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("hello  world"));
        }

        [Test]
        public void Correct_WithMatchingRule_AppliesCorrection()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("hello world"));
        }

        [Test]
        public void Correct_WithNonMatchingRule_ReturnsSameText()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello world");

            Assert.That(result, Is.EqualTo("hello world"));
        }

        [Test]
        public void Correct_MultipleMatchingRules_AppliesAllInOrder()
        {
            var rule1 = new DoubleSpaceRule();

            // Rule 2 uppercases the first character
            var ruleSet = new MultiRuleSet(
                new DoubleSpaceRule(),
                new UppercaseFirstCharRule());

            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            string result = corrector.Correct("hello  world");

            Assert.That(result, Is.EqualTo("Hello world"));
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

        sealed class MultiRuleSet(params IGrammarRule[] rules) : GrammarRuleSet
        {
            readonly IGrammarRule[] rules = rules;

            public override string LanguageCode => "en";
            public override IReadOnlyList<IGrammarRule> Rules => rules;
        }
    }
}
