namespace NuciText.Grammar
{
    /// <summary>
    /// Applies a <see cref="IGrammarRuleSet"/> to a piece of text, producing a corrected result.
    /// </summary>
    public interface IGrammarCorrector
    {
        /// <summary>
        /// Applies all grammar rules from the configured rule set to the given text.
        /// </summary>
        /// <param name="text">The input text to correct.</param>
        /// <returns>The corrected text.</returns>
        string Correct(string text);
    }
}