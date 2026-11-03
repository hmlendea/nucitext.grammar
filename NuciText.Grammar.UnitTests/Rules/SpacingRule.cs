using System;
using NuciText.Grammar.Rules;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests.Rules
{
    [TestFixture]
    public class SpacingRuleTests
    {
        readonly SpacingRule rule = new();

        [Test]
        public void Id_WhenAccessed_ThenReturnsExpectedValue()
            => Assert.That(rule.Id, Is.EqualTo("spacing"));

        [Test]
        public void Description_WhenAccessed_ThenReturnsExpectedValue()
            => Assert.That(rule.Description, Is.EqualTo("Normalises spacing around punctuation and emoticons."));

        [Test]
        public void GivenTextContainsSpacingBeforePunctuation_WhenCanApply_ThenReturnsTrue()
            => Assert.That(rule.CanApply("Salut ,lume"), Is.True);

        [Test]
        public void GivenTextIsAlreadyNormalised_WhenCanApply_ThenReturnsFalse()
            => Assert.That(rule.CanApply("Salut, lume :D"), Is.False);

        [Test]
        public void GivenTextContainsSpacingBeforeComma_WhenApply_ThenRemovesSpacingAndAddsSpaceAfterComma()
        {
            string result = rule.Apply("Salut ,lume");

            Assert.That(result, Is.EqualTo("Salut, lume"));
        }

        [Test]
        public void GivenTextContainsMissingSpaceAfterPunctuation_WhenApply_ThenAddsSpace()
        {
            string result = rule.Apply("Salut:lume.bine;merci");

            Assert.That(result, Is.EqualTo("Salut: lume. bine; merci"));
        }

        [Test]
        public void GivenTextContainsAttachedEmoticon_WhenApply_ThenAddsSpaceBeforeEmoticon()
        {
            string result = rule.Apply("salut:D");

            Assert.That(result, Is.EqualTo("salut :D"));
        }

        [Test]
        public void GivenNullText_WhenApply_ThenThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => rule.Apply(null!));
    }
}