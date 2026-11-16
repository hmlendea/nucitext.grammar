using NuciText.Grammar.UnitTests.Dummies;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public sealed class GrammarRuleSetTests
    {
        [Test]
        public void GivenEmptyRuleSet_WhenInspectingLanguageCode_ThenLanguageCodeIsReturned()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();

            Assert.That(ruleSet.LanguageCode, Is.EqualTo("en"));
        }

        [Test]
        public void GivenEmptyRuleSet_WhenInspectingRules_ThenRulesAreEmpty()
        {
            IGrammarRuleSet ruleSet = new EmptyRuleSet();

            Assert.That(ruleSet.Rules, Is.Empty);
        }

        [Test]
        public void GivenSingleRuleSet_WhenInspectingRules_ThenSingleRuleIsReturned()
        {
            TrimRule rule = new();
            IGrammarRuleSet ruleSet = new SingleRuleSet(rule);

            Assert.That(ruleSet.Rules, Is.EqualTo(new IGrammarRule[] { rule }));
        }

        [Test]
        public void GivenMultipleRules_WhenInspectingRules_ThenDeclaredOrderIsPreserved()
        {
            TrimRule first = new();
            DoubleSpaceRule second = new();
            UppercaseFirstCharRule third = new();

            IGrammarRuleSet ruleSet = new MultiRuleSet(first, second, third);

            Assert.That(ruleSet.Rules, Is.EqualTo(new IGrammarRule[] { first, second, third }));
        }

        [Test]
        public void GivenMultipleRules_WhenInspectingRuleCount_ThenCountMatchesDeclaration()
        {
            IGrammarRuleSet ruleSet = new MultiRuleSet(new TrimRule(), new DoubleSpaceRule());

            Assert.That(ruleSet.Rules, Has.Count.EqualTo(2));
        }

        [Test]
        public void GivenRuleSet_WhenInspectingLanguageCode_ThenLanguageCodeIsNotNull()
        {
            IGrammarRuleSet ruleSet = new MultiRuleSet(new TrimRule());

            Assert.That(ruleSet.LanguageCode, Is.Not.Null);
        }
    }
}