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
        public void Id_ReturnsExpectedValue()
        {
            Assert.That(rule.Id, Is.EqualTo("spacing"));
        }

        [Test]
        public void Description_ReturnsExpectedValue()
        {
            Assert.That(rule.Description, Is.EqualTo("Normalises spacing around punctuation and emoticons."));
        }

        [Test]
        public void CanApply_TextContainsSpacingBeforePunctuation_ReturnsTrue()
        {
            Assert.That(rule.CanApply("Salut ,lume"), Is.True);
        }

        [Test]
        public void CanApply_TextIsAlreadyNormalised_ReturnsFalse()
        {
            Assert.That(rule.CanApply("Salut, lume :D"), Is.False);
        }

        [Test]
        public void Apply_TextContainsSpacingBeforeComma_RemovesSpacingAndAddsSpaceAfterComma()
        {
            string result = rule.Apply("Salut ,lume");

            Assert.That(result, Is.EqualTo("Salut, lume"));
        }

        [Test]
        public void Apply_TextContainsMissingSpaceAfterPunctuation_AddsSpace()
        {
            string result = rule.Apply("Salut:lume.bine;merci");

            Assert.That(result, Is.EqualTo("Salut: lume. bine; merci"));
        }

        [Test]
        public void Apply_TextContainsAttachedEmoticon_AddsSpaceBeforeEmoticon()
        {
            string result = rule.Apply("salut:D");

            Assert.That(result, Is.EqualTo("salut :D"));
        }

        [Test]
        public void Apply_NullText_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => rule.Apply(null!));
        }
    }
}