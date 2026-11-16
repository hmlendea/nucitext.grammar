using NuciText.Grammar.Rules;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests.Rules
{
    [TestFixture]
    public sealed class TrimWhitespaceRuleTests
    {
        readonly TrimWhitespaceRule rule = new();

        [Test]
        public void GivenRule_WhenInspectingId_ThenReturnsTrimWhitespaceIdentifier()
            => Assert.That(rule.Id, Is.EqualTo("trim-whitespace"));

        [Test]
        public void GivenRule_WhenInspectingDescription_ThenReturnsTrimWhitespaceDescription()
            => Assert.That(
                rule.Description,
                Is.EqualTo("Normalises whitespace by trimming and collapsing internal whitespace."));

        [Test]
        public void GivenRule_WhenInspectingType_ThenItDerivesFromGrammarRule()
            => Assert.That(rule, Is.InstanceOf<GrammarRule>());

        [TestCase("  salut  ")]
        [TestCase("salut  lume")]
        [TestCase("\tsalut")]
        [TestCase("salut\n")]
        public void GivenTextWithNonNormalisedWhitespace_WhenCanApply_ThenReturnsTrue(string text)
            => Assert.That(rule.CanApply(text), Is.True);

        [TestCase("salut lume")]
        [TestCase("")]
        [TestCase("salut")]
        public void GivenTextAlreadyNormalised_WhenCanApply_ThenReturnsFalse(string text)
            => Assert.That(rule.CanApply(text), Is.False);

        [TestCase("  salut lume  ", "salut lume")]
        [TestCase("salut lume", "salut lume")]
        [TestCase("  salut   lume  ", "salut lume")]
        [TestCase("\t salut\n\n lume \t", "salut lume")]
        [TestCase("   ", "")]
        [TestCase("", "")]
        [TestCase("\u00A0salut\u00A0", "salut")]
        public void GivenText_WhenApply_ThenTrimsAndCollapsesWhitespace(string text, string expected)
            => Assert.That(rule.Apply(text), Is.EqualTo(expected));

        [Test]
        public void GivenTextWithMixedWhitespaceKinds_WhenApply_ThenOutputUsesSingleSpaces()
        {
            string result = rule.Apply("Salut\r\n\tlume\fmare");

            Assert.That(result, Is.EqualTo("Salut lume mare"));
        }

        [Test]
        public void GivenTextWithLeadingWhitespaceOnly_WhenApply_ThenReturnsContentWithoutLeadingWhitespace()
            => Assert.That(rule.Apply("   salut"), Is.EqualTo("salut"));

        [Test]
        public void GivenTextWithTrailingWhitespaceOnly_WhenApply_ThenReturnsContentWithoutTrailingWhitespace()
            => Assert.That(rule.Apply("salut   "), Is.EqualTo("salut"));

        [Test]
        public void GivenNormalisedText_WhenApplyTwice_ThenSecondApplicationIsStable()
        {
            string once = rule.Apply("  salut   lume  ");
            string twice = rule.Apply(once);

            Assert.That(twice, Is.EqualTo(once));
        }

        [Test]
        public void GivenNullText_WhenCanApply_ThenThrowsArgumentNullException()
            => Assert.That(() => rule.CanApply(null!), Throws.ArgumentNullException);

        [Test]
        public void GivenNullText_WhenApply_ThenThrowsArgumentNullException()
            => Assert.That(() => rule.Apply(null!), Throws.ArgumentNullException);
    }
}
