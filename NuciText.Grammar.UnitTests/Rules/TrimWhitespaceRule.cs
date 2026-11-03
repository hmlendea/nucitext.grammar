using System;
using NuciText.Grammar.Rules;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests.Rules
{
    [TestFixture]
    public class TrimWhitespaceRuleTests
    {
        readonly TrimWhitespaceRule rule = new();

        [Test]
        public void Id_WhenAccessed_ThenReturnsExpectedValue()
            => Assert.That(rule.Id, Is.EqualTo("trim-whitespace"));

        [Test]
        public void Description_WhenAccessed_ThenReturnsExpectedValue()
            => Assert.That(rule.Description, Is.EqualTo("Normalises whitespace by trimming and collapsing internal whitespace."));

        [Test]
        public void GivenTextHasLeadingOrTrailingWhitespace_WhenCanApply_ThenReturnsTrue()
            => Assert.That(rule.CanApply("  salut  "), Is.True);

        [Test]
        public void GivenTextIsAlreadyTrimmed_WhenCanApply_ThenReturnsFalse()
            => Assert.That(rule.CanApply("salut lume"), Is.False);

        [Test]
        public void GivenTextHasLeadingAndTrailingWhitespace_WhenApply_ThenTrimsBothEnds()
        {
            string result = rule.Apply("  salut lume  ");

            Assert.That(result, Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenTextContainsInternalWhitespace_WhenApply_ThenCollapsesInternalWhitespace()
        {
            string result = rule.Apply("  salut   lume  ");

            Assert.That(result, Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenTextContainsTabsAndNewlines_WhenApply_ThenCollapsesWhitespaceToSingleSpaces()
        {
            string result = rule.Apply("\t salut\n\n lume \t");

            Assert.That(result, Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenNullText_WhenApply_ThenThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => rule.Apply(null!));
    }
}
