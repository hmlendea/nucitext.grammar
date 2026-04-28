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
        public void Id_ReturnsExpectedValue()
        {
            Assert.That(rule.Id, Is.EqualTo("trim-whitespace"));
        }

        [Test]
        public void Description_ReturnsExpectedValue()
        {
            Assert.That(rule.Description, Is.EqualTo("Trims leading and trailing whitespace."));
        }

        [Test]
        public void CanApply_TextHasLeadingOrTrailingWhitespace_ReturnsTrue()
        {
            Assert.That(rule.CanApply("  salut  "), Is.True);
        }

        [Test]
        public void CanApply_TextIsAlreadyTrimmed_ReturnsFalse()
        {
            Assert.That(rule.CanApply("salut lume"), Is.False);
        }

        [Test]
        public void Apply_TextHasLeadingAndTrailingWhitespace_TrimsBothEnds()
        {
            string result = rule.Apply("  salut lume  ");

            Assert.That(result, Is.EqualTo("salut lume"));
        }

        [Test]
        public void Apply_TextContainsInternalWhitespace_PreservesInternalWhitespace()
        {
            string result = rule.Apply("  salut   lume  ");

            Assert.That(result, Is.EqualTo("salut   lume"));
        }

        [Test]
        public void Apply_NullText_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => rule.Apply(null!));
        }
    }
}
