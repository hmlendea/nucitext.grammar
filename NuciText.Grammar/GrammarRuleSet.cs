using System.Collections.Generic;

namespace NuciText.Grammar
{
    /// <summary>
    /// Abstract base class for language-specific grammar rule sets.
    /// Derived classes must provide the <see cref="LanguageCode"/> and populate <see cref="Rules"/>.
    /// </summary>
    public abstract class GrammarRuleSet : IGrammarRuleSet
    {
        /// <inheritdoc/>
        public abstract string LanguageCode { get; }

        /// <inheritdoc/>
        public abstract IReadOnlyList<IGrammarRule> Rules { get; }
    }
}