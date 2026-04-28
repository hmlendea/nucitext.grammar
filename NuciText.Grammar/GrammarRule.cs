using System;

namespace NuciText.Grammar
{
    /// <summary>
    /// Abstract base class for grammar correction rules.
    /// Provides a default <see cref="CanApply"/> implementation that checks
    /// whether <see cref="Apply"/> would actually change the text.
    /// Derived classes must implement <see cref="Id"/>, <see cref="Description"/>, and <see cref="Apply"/>.
    /// </summary>
    public abstract class GrammarRule : IGrammarRule
    {
        /// <inheritdoc/>
        public abstract string Id { get; }

        /// <inheritdoc/>
        public abstract string Description { get; }

        /// <inheritdoc/>
        public virtual bool CanApply(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return CheckApplicability(text);
        }

        /// <inheritdoc/>
        public string Apply(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return DoApply(text);
        }

        protected virtual bool CheckApplicability(string text)
            => Apply(text) != text;

        protected abstract string DoApply(string text);
    }
}