using System.Text.RegularExpressions;

namespace NuciText.Grammar.Rules
{
    /// <summary>
    /// Normalises spacing around punctuation and emoticons.
    /// </summary>
    internal sealed class SpacingRule : GrammarRule
    {
        /// <inheritdoc/>
        public override string Id => "spacing";

        /// <inheritdoc/>
        public override string Description => "Normalises spacing around punctuation and emoticons.";

        /// <inheritdoc/>
        protected override string DoApply(string text)
        {
            string result = text;

            result = Regex.Replace(result, "[ ]*([,.])", "$1", RegexOptions.CultureInvariant);
            result = Regex.Replace(result, "([,.:;])([a-zA-Z][a-zA-Z])", "$1 $2", RegexOptions.CultureInvariant);
            result = Regex.Replace(result, "([a-z])([Oo][7/]|[:;=][DdOoPpXx*)]|[Xx][Dd])", "$1 $2", RegexOptions.CultureInvariant);

            return result;
        }
    }
}