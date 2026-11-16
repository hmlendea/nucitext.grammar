using NuciText.Grammar.Rules;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests.Rules
{
    [TestFixture]
    public sealed class SpacingRuleTests
    {
        readonly SpacingRule rule = new();

        [Test]
        public void GivenRule_WhenInspectingId_ThenReturnsSpacingIdentifier()
            => Assert.That(rule.Id, Is.EqualTo("spacing"));

        [Test]
        public void GivenRule_WhenInspectingDescription_ThenReturnsSpacingDescription()
            => Assert.That(
                rule.Description,
                Is.EqualTo("Normalises spacing around punctuation and emoticons."));

        [Test]
        public void GivenRule_WhenInspectingType_ThenItDerivesFromGrammarRule()
            => Assert.That(rule, Is.InstanceOf<GrammarRule>());

        [TestCase("Salut ,lume")]
        [TestCase("Salut .lume")]
        [TestCase("Salut,lume")]
        [TestCase("Salut:lume")]
        [TestCase("salut:D")]
        [TestCase("salut:)")]
        [TestCase("salutO7")]
        [TestCase("salutXD")]
        public void GivenTextNeedingNormalisation_WhenCanApply_ThenReturnsTrue(string text)
            => Assert.That(rule.CanApply(text), Is.True);

        [TestCase("Salut, lume :D")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Salut lume")]
        public void GivenTextAlreadyNormalised_WhenCanApply_ThenReturnsFalse(string text)
            => Assert.That(rule.CanApply(text), Is.False);

        [TestCase("Salut ,lume", "Salut, lume")]
        [TestCase("Salut , lume", "Salut, lume")]
        [TestCase("Salut   ,   lume", "Salut,   lume")]
        [TestCase("Salut,lume", "Salut, lume")]
        [TestCase("Salut:lume.bine;merci", "Salut: lume. bine; merci")]
        [TestCase("Salut, lume", "Salut, lume")]
        public void GivenTextWithPunctuationSpacing_WhenApply_ThenNormalisesSpacing(
            string text,
            string expected)
            => Assert.That(rule.Apply(text), Is.EqualTo(expected));

        [TestCase("salut:D", "salut :D")]
        [TestCase("salut:d", "salut :d")]
        [TestCase("salut;)", "salut ;)")]
        [TestCase("salut=)", "salut =)")]
        [TestCase("salut:P", "salut :P")]
        [TestCase("salut:p", "salut :p")]
        [TestCase("salut:O", "salut :O")]
        [TestCase("salut:o", "salut :o")]
        [TestCase("salut:X", "salut :X")]
        [TestCase("salut:x", "salut :x")]
        [TestCase("salut:*)", "salut :*)")]
        [TestCase("salutXD", "salut XD")]
        [TestCase("salutXd", "salut Xd")]
        [TestCase("salutxD", "salut xD")]
        [TestCase("salutxd", "salut xd")]
        [TestCase("salutO7", "salut O7")]
        [TestCase("saluto7", "salut o7")]
        public void GivenTextWithAttachedEmoticon_WhenApply_ThenAddsSpaceBeforeEmoticon(
            string text,
            string expected)
            => Assert.That(rule.Apply(text), Is.EqualTo(expected));

        [TestCase("SalutLume")]
        [TestCase("Salut123")]
        [TestCase("SALUT")]
        public void GivenTextWithoutPunctuationOrEmoticon_WhenApply_ThenTextIsUnchanged(string text)
            => Assert.That(rule.Apply(text), Is.EqualTo(text));

        [Test]
        public void GivenEmptyText_WhenApply_ThenReturnsEmptyText()
            => Assert.That(rule.Apply(string.Empty), Is.Empty);

        [Test]
        public void GivenNullText_WhenCanApply_ThenThrowsArgumentNullException()
            => Assert.That(() => rule.CanApply(null!), Throws.ArgumentNullException);

        [Test]
        public void GivenNullText_WhenApply_ThenThrowsArgumentNullException()
            => Assert.That(() => rule.Apply(null!), Throws.ArgumentNullException);

        [Test]
        public void GivenAlreadyNormalisedText_WhenApplyTwice_ThenSecondApplicationIsStable()
        {
            string once = rule.Apply("Salut ,lume:D");
            string twice = rule.Apply(once);

            Assert.That(twice, Is.EqualTo(once));
        }
    }
}