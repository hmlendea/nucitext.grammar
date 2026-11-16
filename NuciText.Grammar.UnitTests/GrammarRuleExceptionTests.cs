using System;
using NUnit.Framework;

namespace NuciText.Grammar.UnitTests
{
    [TestFixture]
    public sealed class GrammarRuleExceptionTests
    {
        [Test]
        public void GivenRuleId_WhenConstructing_ThenRuleIdIsPreserved()
        {
            GrammarRuleException exception = new("spacing");

            Assert.That(exception.RuleId, Is.EqualTo("spacing"));
        }

        [Test]
        public void GivenRuleId_WhenConstructing_ThenDefaultMessageDescribesTheRule()
        {
            GrammarRuleException exception = new("spacing");

            Assert.That(
                exception.Message,
                Is.EqualTo("Grammar rule 'spacing' failed to apply."));
        }

        [Test]
        public void GivenRuleId_WhenConstructing_ThenNoInnerExceptionIsAttached()
        {
            GrammarRuleException exception = new("spacing");

            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void GivenRuleIdAndMessage_WhenConstructing_ThenMessageIsPreserved()
        {
            GrammarRuleException exception = new("spacing", "Custom failure.");

            Assert.That(exception.Message, Is.EqualTo("Custom failure."));
        }

        [Test]
        public void GivenRuleIdAndMessage_WhenConstructing_ThenRuleIdIsPreserved()
        {
            GrammarRuleException exception = new("spacing", "Custom failure.");

            Assert.That(exception.RuleId, Is.EqualTo("spacing"));
        }

        [Test]
        public void GivenRuleIdAndInnerException_WhenConstructing_ThenDefaultMessageIsUsed()
        {
            InvalidOperationException inner = new("Inner failure.");
            GrammarRuleException exception = new("spacing", inner);

            Assert.That(
                exception.Message,
                Is.EqualTo("Grammar rule 'spacing' failed to apply."));
        }

        [Test]
        public void GivenRuleIdAndInnerException_WhenConstructing_ThenInnerExceptionIsPreserved()
        {
            InvalidOperationException inner = new("Inner failure.");
            GrammarRuleException exception = new("spacing", inner);

            Assert.That(exception.InnerException, Is.SameAs(inner));
        }

        [Test]
        public void GivenRuleIdMessageAndInnerException_WhenConstructing_ThenAllValuesArePreserved()
        {
            InvalidOperationException inner = new("Inner failure.");
            GrammarRuleException exception = new("spacing", "Custom failure.", inner);

            Assert.That(exception.RuleId, Is.EqualTo("spacing"));
            Assert.That(exception.Message, Is.EqualTo("Custom failure."));
            Assert.That(exception.InnerException, Is.SameAs(inner));
        }

        [Test]
        public void GivenNullRuleId_WhenConstructing_ThenMessageOmitsTheIdentifier()
        {
            GrammarRuleException exception = new(null!);

            Assert.That(exception.Message, Is.EqualTo("Grammar rule '' failed to apply."));
        }

        [Test]
        public void GivenException_WhenInspectingType_ThenItDerivesFromException()
        {
            GrammarRuleException exception = new("spacing");

            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}