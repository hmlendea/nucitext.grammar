using System.Text.RegularExpressions;

namespace NuciText.Grammar.Rules
{
    /// <summary>
    /// Trims leading and trailing whitespace from Romanian text.
    /// </summary>
    public sealed class TrimWhitespaceRule : GrammarRule
    {
        /// <inheritdoc/>
        public override string Id => "trim-whitespace";

        /// <inheritdoc/>
        public override string Description => "Normalises whitespace by trimming and collapsing internal whitespace.";

        /// <inheritdoc/>
        protected override string DoApply(string text)
            => Regex.Replace(text.Trim(), "\\s+", " ", RegexOptions.CultureInvariant);
    }
}