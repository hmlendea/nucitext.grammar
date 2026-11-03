using System;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public class GrammarRuleTests
    {
        sealed class TrimRule : GrammarRule
        {
            public override string Id => "trim";
            public override string Description => "Trims leading and trailing whitespace.";
            protected override string DoApply(string text) => text.Trim();
        }

        [Test]
        public void GivenTextChangedByApply_WhenCanApply_ThenReturnsTrue()
        {
            IGrammarRule rule = new TrimRule();
            Assert.That(rule.CanApply("  hello  "), Is.True);
        }

        [Test]
        public void GivenTextNotChangedByApply_WhenCanApply_ThenReturnsFalse()
        {
            IGrammarRule rule = new TrimRule();
            Assert.That(rule.CanApply("hello"), Is.False);
        }

        [Test]
        public void GivenNullText_WhenCanApply_ThenThrowsArgumentNullException()
        {
            IGrammarRule rule = new TrimRule();
            Assert.Throws<ArgumentNullException>(() => rule.CanApply(null!));
        }

        [Test]
        public void GivenTextWithWhitespace_WhenApply_ThenProducesExpectedResult()
        {
            IGrammarRule rule = new TrimRule();
            Assert.That(rule.Apply("  hello  "), Is.EqualTo("hello"));
        }
    }
}