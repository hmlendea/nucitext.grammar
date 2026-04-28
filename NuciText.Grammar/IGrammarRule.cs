namespace NuciText.Grammar;

/// <summary>
/// Represents a single grammar correction rule that can detect and fix a specific pattern in text.
/// </summary>
public interface IGrammarRule
{
    /// <summary>
    /// Gets the unique identifier of this rule.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets a human-readable description of what this rule corrects.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Determines whether this rule can be applied to the given text.
    /// </summary>
    /// <param name="text">The input text to evaluate.</param>
    /// <returns><c>true</c> if the rule matches the text; otherwise, <c>false</c>.</returns>
    bool CanApply(string text);

    /// <summary>
    /// Applies the grammar correction defined by this rule to the given text.
    /// </summary>
    /// <param name="text">The input text to correct.</param>
    /// <returns>The corrected text.</returns>
    string Apply(string text);
}
