using NuciText.Grammar.UnitTests.Dummies;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public sealed class GrammarRuleTests
    {
        [Test]
        public void GivenRule_WhenInspectingId_ThenIdentifierIsReturned()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(rule.Id, Is.EqualTo("trim"));
        }

        [Test]
        public void GivenRule_WhenInspectingDescription_ThenDescriptionIsReturned()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(rule.Description, Is.EqualTo("Trims leading and trailing whitespace."));
        }

        [TestCase("  salut  ")]
        [TestCase(" salut")]
        [TestCase("salut ")]
        [TestCase("\tsalut")]
        public void GivenTextChangedByApply_WhenCanApply_ThenReturnsTrue(string text)
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(rule.CanApply(text), Is.True);
        }

        [TestCase("salut")]
        [TestCase("")]
        [TestCase("salut lume")]
        public void GivenTextNotChangedByApply_WhenCanApply_ThenReturnsFalse(string text)
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(rule.CanApply(text), Is.False);
        }

        [Test]
        public void GivenNullText_WhenCanApply_ThenThrowsArgumentNullException()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(() => rule.CanApply(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void GivenNullText_WhenCanApply_ThenExceptionIdentifiesTextParameter()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(
                () => rule.CanApply(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("text"));
        }

        [Test]
        public void GivenNullText_WhenApply_ThenThrowsArgumentNullException()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(() => rule.Apply(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void GivenNullText_WhenApply_ThenExceptionIdentifiesTextParameter()
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(
                () => rule.Apply(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("text"));
        }

        [TestCase("  salut  ", "salut")]
        [TestCase("", "")]
        [TestCase("salut", "salut")]
        [TestCase("\r\nsalut\t", "salut")]
        public void GivenText_WhenApply_ThenReturnsDoApplyResult(string text, string expected)
        {
            IGrammarRule rule = new TrimRule();

            Assert.That(rule.Apply(text), Is.EqualTo(expected));
        }

        [Test]
        public void GivenApplicabilityOverrideFalse_WhenCanApply_ThenReturnsFalse()
        {
            IGrammarRule rule = new FixedApplicabilityRule(false);

            Assert.That(rule.CanApply("salut"), Is.False);
        }

        [Test]
        public void GivenApplicabilityOverrideFalse_WhenApply_ThenCorrectionIsStillApplied()
        {
            IGrammarRule rule = new FixedApplicabilityRule(false);

            Assert.That(rule.Apply("salut"), Is.EqualTo("salut!"));
        }

        [Test]
        public void GivenApplicabilityOverrideTrue_WhenCanApply_ThenReturnsTrueEvenIfTextIsUnchanged()
        {
            IGrammarRule rule = new FixedApplicabilityRule(true);

            Assert.That(rule.CanApply(string.Empty), Is.True);
        }

        [Test]
        public void GivenRule_WhenInspectingType_ThenItImplementsRuleInterface()
        {
            GrammarRule rule = new TrimRule();

            Assert.That(rule, Is.InstanceOf<IGrammarRule>());
        }
    }
}