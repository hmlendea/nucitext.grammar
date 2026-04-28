using System;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests;

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
    public void CanApply_TextChangedByApply_ReturnsTrue()
    {
        IGrammarRule rule = new TrimRule();
        Assert.That(rule.CanApply("  hello  "), Is.True);
    }

    [Test]
    public void CanApply_TextNotChangedByApply_ReturnsFalse()
    {
        IGrammarRule rule = new TrimRule();
        Assert.That(rule.CanApply("hello"), Is.False);
    }

    [Test]
    public void CanApply_NullText_ThrowsArgumentNullException()
    {
        IGrammarRule rule = new TrimRule();
        Assert.Throws<ArgumentNullException>(() => rule.CanApply(null!));
    }

    [Test]
    public void Apply_ProducesExpectedResult()
    {
        IGrammarRule rule = new TrimRule();
        Assert.That(rule.Apply("  hello  "), Is.EqualTo("hello"));
    }
}
