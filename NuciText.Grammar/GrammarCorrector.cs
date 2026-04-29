using System;

namespace NuciText.Grammar
{
    /// <summary>
    /// Applies all rules from a <see cref="IGrammarRuleSet"/> to a piece of text in order,
    /// producing a fully corrected result.
    /// </summary>
    /// <remarks>
    /// Initialises a new <see cref="GrammarCorrector"/> with the given rule set.
    /// </remarks>
    /// <param name="ruleSet">The rule set to use for corrections.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ruleSet"/> is null.</exception>
    public sealed class GrammarCorrector(IGrammarRuleSet ruleSet) : IGrammarCorrector
    {
        readonly IGrammarRuleSet ruleSet = ruleSet ?? throw new ArgumentNullException(nameof(ruleSet));

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
        public string Correct(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            string result = text;

            foreach (IGrammarRule rule in ruleSet.Rules)
            {
                if (rule.CanApply(result))
                {
                    result = rule.Apply(result);
                }
            }

            return result;
        }
    }
}