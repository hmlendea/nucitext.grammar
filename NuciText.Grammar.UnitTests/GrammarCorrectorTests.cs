using System;
using System.Collections.Generic;
using Moq;
using NuciText.Grammar.UnitTests.Dummies;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public sealed class GrammarCorrectorTests
    {
        [Test]
        public void GivenNullRuleSet_WhenConstructing_ThenThrowsArgumentNullException()
            => Assert.That(() => new GrammarCorrector(null!), Throws.ArgumentNullException);

        [Test]
        public void GivenNullRuleSet_WhenConstructing_ThenExceptionIdentifiesRuleSetParameter()
            => Assert.That(
                () => new GrammarCorrector(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("ruleSet"));

        [Test]
        public void GivenEmptyRuleSet_WhenCorrectingNullText_ThenThrowsArgumentNullException()
        {
            IGrammarCorrector corrector = new GrammarCorrector(new EmptyRuleSet());

            Assert.That(() => corrector.Correct(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void GivenEmptyRuleSet_WhenCorrectingNullText_ThenExceptionIdentifiesTextParameter()
        {
            IGrammarCorrector corrector = new GrammarCorrector(new EmptyRuleSet());

            Assert.That(
                () => corrector.Correct(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("text"));
        }

        [TestCase("salut  lume")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Salut, lume.")]
        public void GivenEmptyRuleSet_WhenCorrectingText_ThenReturnsSameText(string text)
        {
            IGrammarCorrector corrector = new GrammarCorrector(new EmptyRuleSet());

            Assert.That(corrector.Correct(text), Is.EqualTo(text));
        }

        [Test]
        public void GivenEmptyRuleSet_WhenCorrectingText_ThenResultIsNotNull()
        {
            IGrammarCorrector corrector = new GrammarCorrector(new EmptyRuleSet());

            Assert.That(corrector.Correct("salut"), Is.Not.Null);
        }

        [Test]
        public void GivenMatchingRule_WhenCorrectingText_ThenAppliesCorrection()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            Assert.That(corrector.Correct("salut  lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenNonMatchingRule_WhenCorrectingText_ThenReturnsSameText()
        {
            IGrammarRuleSet ruleSet = new SingleRuleSet(new DoubleSpaceRule());
            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            Assert.That(corrector.Correct("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenMultipleMatchingRules_WhenCorrectingText_ThenAppliesAllInOrder()
        {
            IGrammarRuleSet ruleSet = new MultiRuleSet(
                new DoubleSpaceRule(),
                new UppercaseFirstCharRule());

            IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

            Assert.That(corrector.Correct("salut  lume"), Is.EqualTo("Salut lume"));
        }

        [Test]
        public void GivenTwoRules_WhenCorrectingText_ThenSecondRuleReceivesFirstRuleOutput()
        {
            Mock<IGrammarRule> firstRule = new(MockBehavior.Strict);
            Mock<IGrammarRule> secondRule = new(MockBehavior.Strict);

            firstRule.Setup(rule => rule.CanApply("salut lume")).Returns(true);
            firstRule.Setup(rule => rule.Apply("salut lume")).Returns("Salut lume");
            secondRule.Setup(rule => rule.CanApply("Salut lume")).Returns(true);
            secondRule.Setup(rule => rule.Apply("Salut lume")).Returns("Salut lume!");

            IGrammarCorrector corrector = new GrammarCorrector(
                new MultiRuleSet(firstRule.Object, secondRule.Object));

            Assert.That(corrector.Correct("salut lume"), Is.EqualTo("Salut lume!"));
        }

        [Test]
        public void GivenTwoRules_WhenCorrectingText_ThenRulesAreInvokedInDeclaredOrder()
        {
            MockSequence sequence = new();
            Mock<IGrammarRule> firstRule = new(MockBehavior.Strict);
            Mock<IGrammarRule> secondRule = new(MockBehavior.Strict);

            firstRule.InSequence(sequence).Setup(rule => rule.CanApply("salut")).Returns(true);
            firstRule.InSequence(sequence).Setup(rule => rule.Apply("salut")).Returns("Salut");
            secondRule.InSequence(sequence).Setup(rule => rule.CanApply("Salut")).Returns(true);
            secondRule.InSequence(sequence).Setup(rule => rule.Apply("Salut")).Returns("Salut!");

            IGrammarCorrector corrector = new GrammarCorrector(
                new MultiRuleSet(firstRule.Object, secondRule.Object));

            corrector.Correct("salut");

            firstRule.VerifyAll();
            secondRule.VerifyAll();
        }

        [Test]
        public void GivenRuleThatCannotApply_WhenCorrectingText_ThenApplyIsNeverInvoked()
        {
            Mock<IGrammarRule> rule = new(MockBehavior.Strict);
            rule.Setup(candidate => candidate.CanApply("salut")).Returns(false);

            IGrammarCorrector corrector = new GrammarCorrector(new SingleRuleSet(rule.Object));

            Assert.That(corrector.Correct("salut"), Is.EqualTo("salut"));
        }

        [Test]
        public void GivenRepeatedRule_WhenCorrectingText_ThenCanApplyIsEvaluatedOnCurrentText()
        {
            List<string> observedInputs = [];

            Mock<IGrammarRule> rule = new();
            rule.Setup(candidate => candidate.CanApply(It.IsAny<string>()))
                .Callback<string>(observedInputs.Add)
                .Returns(true);
            rule.Setup(candidate => candidate.Apply(It.IsAny<string>()))
                .Returns((string text) => text + "!");

            IGrammarCorrector corrector = new GrammarCorrector(
                new MultiRuleSet(rule.Object, rule.Object));

            Assert.That(corrector.Correct("salut"), Is.EqualTo("salut!!"));
            Assert.That(observedInputs, Is.EqualTo(new[] { "salut", "salut!" }));
        }

        [Test]
        public void GivenRuleThatThrowsOnCanApply_WhenCorrectingText_ThenExceptionPropagates()
        {
            Mock<IGrammarRule> rule = new(MockBehavior.Strict);
            rule.Setup(candidate => candidate.CanApply("salut"))
                .Throws(new GrammarRuleException("spacing"));

            IGrammarCorrector corrector = new GrammarCorrector(new SingleRuleSet(rule.Object));

            Assert.That(
                () => corrector.Correct("salut"),
                Throws.TypeOf<GrammarRuleException>()
                    .With.Property("RuleId").EqualTo("spacing"));
        }

        [Test]
        public void GivenRuleThatThrowsOnApply_WhenCorrectingText_ThenExceptionPropagates()
        {
            Mock<IGrammarRule> rule = new(MockBehavior.Strict);
            rule.Setup(candidate => candidate.CanApply("salut")).Returns(true);
            rule.Setup(candidate => candidate.Apply("salut"))
                .Throws(new GrammarRuleException("spacing", "Failure."));

            IGrammarCorrector corrector = new GrammarCorrector(new SingleRuleSet(rule.Object));

            Assert.That(
                () => corrector.Correct("salut"),
                Throws.TypeOf<GrammarRuleException>().With.Message.EqualTo("Failure."));
        }

        [Test]
        public void GivenFailingRule_WhenCorrectingText_ThenSubsequentRulesAreNotInvoked()
        {
            Mock<IGrammarRule> failingRule = new(MockBehavior.Strict);
            Mock<IGrammarRule> subsequentRule = new(MockBehavior.Strict);

            failingRule.Setup(candidate => candidate.CanApply("salut")).Returns(true);
            failingRule.Setup(candidate => candidate.Apply("salut"))
                .Throws(new GrammarRuleException("spacing"));

            IGrammarCorrector corrector = new GrammarCorrector(
                new MultiRuleSet(failingRule.Object, subsequentRule.Object));

            Assert.That(() => corrector.Correct("salut"), Throws.TypeOf<GrammarRuleException>());
            subsequentRule.VerifyNoOtherCalls();
        }

        [Test]
        public void GivenApplicabilityOverrideFalse_WhenCorrectingText_ThenRuleIsSkipped()
        {
            IGrammarCorrector corrector = new GrammarCorrector(
                new SingleRuleSet(new FixedApplicabilityRule(false)));

            Assert.That(corrector.Correct("salut"), Is.EqualTo("salut"));
        }

        [Test]
        public void GivenApplicabilityOverrideTrue_WhenCorrectingText_ThenRuleIsApplied()
        {
            IGrammarCorrector corrector = new GrammarCorrector(
                new SingleRuleSet(new FixedApplicabilityRule(true)));

            Assert.That(corrector.Correct("salut"), Is.EqualTo("salut!"));
        }

        [Test]
        public void GivenRuleSet_WhenCorrectingTwice_ThenBothCallsProduceTheSameResult()
        {
            IGrammarCorrector corrector = new GrammarCorrector(new SingleRuleSet(new TrimRule()));

            string first = corrector.Correct("  salut  ");
            string second = corrector.Correct("  salut  ");

            Assert.That(second, Is.EqualTo(first));
        }

        [Test]
        public void GivenCorrector_WhenCorrectingAlreadyCorrectedText_ThenTextIsUnchanged()
        {
            IGrammarCorrector corrector = new GrammarCorrector(
                new MultiRuleSet(new TrimRule(), new UppercaseFirstCharRule()));

            string once = corrector.Correct("  salut  lume  ");
            string twice = corrector.Correct(once);

            Assert.That(twice, Is.EqualTo(once));
        }

        [Test]
        public void GivenCorrector_WhenInspectingType_ThenItImplementsCorrectorInterface()
        {
            GrammarCorrector corrector = new(new EmptyRuleSet());

            Assert.That(corrector, Is.InstanceOf<IGrammarCorrector>());
        }
    }
}
