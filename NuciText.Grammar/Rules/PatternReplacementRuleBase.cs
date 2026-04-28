using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NuciText.Grammar.Rules
{
    /// <summary>
    /// Represents a pattern replacement using regular expressions.
    /// </summary>
    /// <param name="Pattern">The regular expression pattern to match.</param>
    /// <param name="Replacement">The replacement string.</param>
    public readonly record struct RegexReplacement(string Pattern, string Replacement);

    /// <summary>
    /// Base class for grammar rules that perform pattern-based replacements using regular expressions.
    /// </summary>
    public abstract class PatternReplacementRuleBase : GrammarRule
    {
        static readonly char[] WordTerminators = [' ', '?', '!', '.', '-'];

        /// <summary>
        /// Applies a series of word replacements to the given text, ensuring that only whole words are replaced.
        /// </summary>
        /// <param name="text">The text to apply the replacements to.</param>
        /// <param name="replacements">The collection of word replacements to apply.</param>
        /// <returns>The text with the word replacements applied.</returns>
        protected static string ApplyWordReplacements(string text, IEnumerable<RegexReplacement> replacements)
        {
            string result = text;

            foreach (RegexReplacement replacement in replacements)
            {
                result = ReplaceWord(result, replacement.Pattern, replacement.Replacement);
            }

            return result;
        }

        /// <summary>
        /// Applies a series of regular expression replacements to the given text.
        /// </summary>
        /// <param name="text">The text to apply the replacements to.</param>
        /// <param name="replacements">The collection of regular expression replacements to apply.</param>
        /// <returns>The text with the regular expression replacements applied.</returns>
        protected static string ApplyRegexReplacements(string text, IEnumerable<RegexReplacement> replacements)
        {
            string result = text;

            foreach (RegexReplacement replacement in replacements)
            {
                result = Regex.Replace(result, replacement.Pattern, replacement.Replacement, RegexOptions.CultureInvariant);
            }

            return result;
        }

        /// <summary>
        /// Replaces whole words in the given text that match the specified pattern with the replacement string.
        /// </summary>
        /// <param name="text">The text to apply the replacement to.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <returns>The text with the word replacements applied.</returns>
        protected static string ReplaceWord(string text, string pattern, string replacement)
        {
            string result = text;

            result = Regex.Replace(result, $"^{pattern}$", replacement, RegexOptions.CultureInvariant);
            result = Regex.Replace(result, $" {pattern}$", $" {replacement}", RegexOptions.CultureInvariant);

            foreach (char terminator in WordTerminators)
            {
                string escapedTerminator = Regex.Escape(terminator.ToString());
                result = Regex.Replace(result, $"^{pattern}{escapedTerminator}", $"{replacement}{terminator}", RegexOptions.CultureInvariant);
                result = Regex.Replace(result, $" {pattern}{escapedTerminator}", $" {replacement}{terminator}", RegexOptions.CultureInvariant);
            }

            return result;
        }
    }
}