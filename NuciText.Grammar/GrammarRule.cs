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

        /// <summary>
        /// Checks whether applying this rule would change the text.
        /// By default, this compares the result of <see cref="Apply"/> to the original text.
        /// Derived classes can override this for more efficient applicability checks if needed.
        /// </summary> <param name="text">The input text to check.</param>
        /// <returns><c>true</c> if applying the rule would change the text; otherwise, <c>false</c>.</returns>
        protected virtual bool CheckApplicability(string text)
            => DoApply(text) != text;

        /// <summary>
        /// Performs the actual grammar correction defined by this rule.
        /// Derived classes must implement this to define the specific correction logic.
        /// </summary> <param name="text">The input text to correct.</param>
        /// <returns>The corrected text.</returns>
        protected abstract string DoApply(string text);
    }
}