using NuciText.Grammar.Rules;
using NuciText.Grammar.UnitTests.Dummies;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests.Rules
{
    [TestFixture]
    public sealed class PatternReplacementRuleBaseTests
    {
        [Test]
        public void GivenWordAtStartOfText_WhenReplacing_ThenWordIsReplaced()
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("bună lume"));
        }

        [Test]
        public void GivenWholeTextMatchingWord_WhenReplacing_ThenWordIsReplaced()
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply("salut"), Is.EqualTo("bună"));
        }

        [TestCase("salut!", "bună!")]
        [TestCase("salut?", "bună?")]
        [TestCase("salut.", "bună.")]
        [TestCase("salut-", "bună-")]
        public void GivenWordAtStartFollowedByTerminator_WhenReplacing_ThenWordIsReplaced(
            string text,
            string expected)
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply(text), Is.EqualTo(expected));
        }

        [TestCase("Salut salut", "Salut bună")]
        [TestCase("Salut salut.", "Salut bună.")]
        [TestCase("Salut salut!", "Salut bună!")]
        public void GivenWordAtEndOfText_WhenReplacing_ThenWordIsReplaced(
            string text,
            string expected)
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply(text), Is.EqualTo(expected));
        }

        [TestCase("Salut, salut. Lume", "Salut, bună. Lume")]
        [TestCase("Salut, salut? Lume", "Salut, bună? Lume")]
        [TestCase("Salut, salut! Lume", "Salut, bună! Lume")]
        [TestCase("Salut, salut- Lume", "Salut, bună- Lume")]
        [TestCase("Salut, salut Lume", "Salut, bună Lume")]
        public void GivenWordFollowedByTerminatorMidText_WhenReplacing_ThenWordIsReplaced(
            string text,
            string expected)
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply(text), Is.EqualTo(expected));
        }

        [TestCase("salutare")]
        [TestCase("bunăsalut")]
        [TestCase("Salut, salutare")]
        public void GivenPartialWordMatch_WhenReplacing_ThenTextIsUnchanged(string text)
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.Apply(text), Is.EqualTo(text));
        }

        [Test]
        public void GivenNoMatch_WhenCanApply_ThenReturnsFalse()
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.CanApply("lume"), Is.False);
        }

        [Test]
        public void GivenMatch_WhenCanApply_ThenReturnsTrue()
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(rule.CanApply("salut"), Is.True);
        }

        [Test]
        public void GivenMultipleWordReplacements_WhenApplying_ThenAllReplacementAreAppliedInOrder()
        {
            WordReplacementRule rule = new(
                new RegexReplacement("salut", "bună"),
                new RegexReplacement("lume", "pământ"));

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("bună pământ"));
        }

        [Test]
        public void GivenChainedWordReplacements_WhenApplying_ThenLaterReplacementSeesEarlierOutput()
        {
            WordReplacementRule rule = new(
                new RegexReplacement("salut", "bună"),
                new RegexReplacement("bună", "excelent"));

            Assert.That(rule.Apply("salut"), Is.EqualTo("excelent"));
        }

        [Test]
        public void GivenNoWordReplacements_WhenApplying_ThenTextIsUnchanged()
        {
            WordReplacementRule rule = new();

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenRegexReplacement_WhenApplying_ThenPatternIsReplaced()
        {
            RegexReplacementRule rule = new(new RegexReplacement("\\s+", " "));

            Assert.That(rule.Apply("salut   lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenRegexReplacement_WhenApplying_ThenReplacementIsAppliedEverywhere()
        {
            RegexReplacementRule rule = new(new RegexReplacement("a", "e"));

            Assert.That(rule.Apply("salata"), Is.EqualTo("selete"));
        }

        [Test]
        public void GivenMultipleRegexReplacements_WhenApplying_ThenAllReplacementsAreAppliedInOrder()
        {
            RegexReplacementRule rule = new(
                new RegexReplacement("a", "e"),
                new RegexReplacement("e", "i"));

            Assert.That(rule.Apply("salata"), Is.EqualTo("siliti"));
        }

        [Test]
        public void GivenRegexReplacementWithCaptureGroup_WhenApplying_ThenCaptureIsExpanded()
        {
            RegexReplacementRule rule = new(new RegexReplacement("(\\w+) (\\w+)", "$2 $1"));

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("lume salut"));
        }

        [Test]
        public void GivenRegexReplacementThatMatchesNothing_WhenApplying_ThenTextIsUnchanged()
        {
            RegexReplacementRule rule = new(new RegexReplacement("xyz", "abc"));

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenNoRegexReplacements_WhenApplying_ThenTextIsUnchanged()
        {
            RegexReplacementRule rule = new();

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenNullText_WhenReplacingWord_ThenThrowsArgumentNullException()
        {
            SingleWordReplacementRule rule = new("salut", "bună");

            Assert.That(() => rule.Apply(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void GivenIdenticalPatternAndReplacement_WhenApplying_ThenTextIsUnchanged()
        {
            SingleWordReplacementRule rule = new("salut", "salut");

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenIdenticalRegexPatternAndReplacement_WhenApplying_ThenTextIsUnchanged()
        {
            RegexReplacementRule rule = new(new RegexReplacement("salut", "salut"));

            Assert.That(rule.Apply("salut lume"), Is.EqualTo("salut lume"));
        }

        [Test]
        public void GivenEqualReplacementValues_WhenComparing_ThenReplacementAreEqual()
        {
            RegexReplacement first = new("salut", "bună");
            RegexReplacement second = new("salut", "bună");

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void GivenDifferentReplacementValues_WhenComparing_ThenReplacementAreNotEqual()
        {
            RegexReplacement first = new("salut", "bună");
            RegexReplacement second = new("salut", "excelent");

            Assert.That(first, Is.Not.EqualTo(second));
        }
    }
}