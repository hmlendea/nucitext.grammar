using System.Collections.Generic;

namespace NuciText.Grammar;

/// <summary>
/// Represents an ordered collection of grammar correction rules for a specific language.
/// </summary>
public interface IGrammarRuleSet
{
    /// <summary>
    /// Gets the IETF language tag (e.g. "en", "ro", "de") this rule set applies to.
    /// </summary>
    string LanguageCode { get; }

    /// <summary>
    /// Gets the ordered list of grammar rules in this rule set.
    /// Rules are applied in the order they are returned.
    /// </summary>
    IReadOnlyList<IGrammarRule> Rules { get; }
}
